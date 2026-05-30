using Microsoft.EntityFrameworkCore;
using Oxygen.Data;
using Oxygen.DTOs;
using Oxygen.Interfaces;
using Oxygen.Models;

namespace Oxygen.Services
{
    public class LookupService : ILookupService
    {
        private readonly AppDbContext _dbContext;

        public LookupService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<LookupListDto>> GetAsync(string? type, CancellationToken cancellationToken)
        {
            var query = _dbContext.LookupLists.AsQueryable();
            if (!string.IsNullOrWhiteSpace(type))
            {
                var normalizedType = type.Trim();
                query = query.Where(item => item.Type == normalizedType);
            }

            var items = await query
                .OrderBy(item => item.Type)
                .ThenBy(item => item.Value)
                .ToListAsync(cancellationToken);

            return items.Select(item => new LookupListDto
            {
                Id = item.Id,
                Type = item.Type,
                Value = item.Value
            }).ToList();
        }

        public async Task<LookupListDto> CreateAsync(CreateLookupListDto request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Type) || string.IsNullOrWhiteSpace(request.Value))
            {
                throw new ArgumentException("type and value are required");
            }

            var normalizedType = request.Type.Trim();
            var normalizedValue = request.Value.Trim();

            var entity = new LookupList
            {
                Id = Guid.NewGuid(),
                Type = normalizedType,
                Value = normalizedValue
            };

            _dbContext.LookupLists.Add(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new LookupListDto
            {
                Id = entity.Id,
                Type = entity.Type,
                Value = entity.Value
            };
        }

        public async Task<LookupListDto?> UpdateAsync(
            Guid id,
            CreateLookupListDto request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Type) || string.IsNullOrWhiteSpace(request.Value))
            {
                throw new ArgumentException("type and value are required");
            }

            var entity = await _dbContext.LookupLists
                .FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
            if (entity is null)
            {
                return null;
            }

            entity.Type = request.Type.Trim();
            entity.Value = request.Value.Trim();
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new LookupListDto
            {
                Id = entity.Id,
                Type = entity.Type,
                Value = entity.Value
            };
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.LookupLists
                .FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
            if (entity is null)
            {
                return false;
            }

            _dbContext.LookupLists.Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
