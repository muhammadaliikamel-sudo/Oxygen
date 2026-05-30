using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using Oxygen.Data;
using Oxygen.DTOs;
using Oxygen.Helpers;
using Oxygen.Interfaces;
using Oxygen.Models;

namespace Oxygen.Services
{
    public class DocumentService : IDocumentService
    {
        private static readonly HashSet<string> AllowedBuckets = new(StringComparer.OrdinalIgnoreCase)
        {
            "resumes",
            "badge-photos",
            "exports",
            "mou-files"
        };

        private readonly HttpClient _httpClient;
        private readonly AppDbContext _dbContext;
        private readonly SupabaseStorageOptions _options;

        public DocumentService(
            HttpClient httpClient,
            AppDbContext dbContext,
            IOptions<SupabaseStorageOptions> options)
        {
            _httpClient = httpClient;
            _dbContext = dbContext;
            _options = options.Value;
        }

        public async Task<DocumentUploadResponseDto> UploadAsync(
            DocumentUploadRequestDto request,
            CancellationToken cancellationToken)
        {
            ValidateRequest(request);

            var bucket = request.Bucket.Trim();
            var fileName = Path.GetFileName(request.File.FileName);
            var objectPath = $"{Guid.NewGuid()}/{fileName}";

            var endpoint = $"{_options.Url}/storage/v1/object/{bucket}/{objectPath}?upsert=true";
            using var uploadRequest = new HttpRequestMessage(HttpMethod.Post, endpoint);
            uploadRequest.Headers.Add("apikey", _options.ServiceRoleKey);
            uploadRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.ServiceRoleKey);

            await using var fileStream = request.File.OpenReadStream();
            var content = new StreamContent(fileStream);
            content.Headers.ContentType = new MediaTypeHeaderValue(request.File.ContentType);
            uploadRequest.Content = content;

            using var response = await _httpClient.SendAsync(uploadRequest, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new InvalidOperationException($"Storage upload failed: {error}");
            }

            var publicBaseUrl = string.IsNullOrWhiteSpace(_options.PublicUrl)
                ? _options.Url
                : _options.PublicUrl;
            var fileUrl = $"{publicBaseUrl}/storage/v1/object/public/{bucket}/{objectPath}";

            var document = new Document
            {
                Id = Guid.NewGuid(),
                FileName = fileName,
                BucketName = bucket,
                FileUrl = fileUrl,
                MimeType = string.IsNullOrWhiteSpace(request.File.ContentType)
                    ? "application/octet-stream"
                    : request.File.ContentType,
                Size = request.File.Length,
                UploadedBy = request.UploadedBy ?? Guid.Empty,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.Documents.Add(document);

            if (!string.IsNullOrWhiteSpace(request.EntityType) && request.EntityId.HasValue)
            {
                var entityDocument = new EntityDocument
                {
                    Id = Guid.NewGuid(),
                    DocumentId = document.Id,
                    EntityType = request.EntityType.Trim(),
                    EntityId = request.EntityId.Value,
                    CreatedAt = DateTime.UtcNow
                };

                _dbContext.EntityDocuments.Add(entityDocument);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new DocumentUploadResponseDto
            {
                DocumentId = document.Id,
                FileUrl = fileUrl,
                Bucket = bucket,
                FileName = fileName
            };
        }

        private static void ValidateRequest(DocumentUploadRequestDto request)
        {
            if (request.File is null || request.File.Length == 0)
            {
                throw new ArgumentException("file is required");
            }

            if (string.IsNullOrWhiteSpace(request.Bucket))
            {
                throw new ArgumentException("bucket is required");
            }

            if (!AllowedBuckets.Contains(request.Bucket.Trim()))
            {
                throw new ArgumentException("bucket is not allowed");
            }
        }
    }
}
