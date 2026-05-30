using Oxygen.DTOs;

namespace Oxygen.Interfaces
{
    public interface IDocumentService
    {
        Task<DocumentUploadResponseDto> UploadAsync(DocumentUploadRequestDto request, CancellationToken cancellationToken);
    }
}
