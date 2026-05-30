using Oxygen.DTOs;

namespace Oxygen.Interfaces
{
    public interface ILookupService
    {
        Task<IReadOnlyList<LookupListDto>> GetAsync(string? type, CancellationToken cancellationToken);
        Task<LookupListDto> CreateAsync(CreateLookupListDto request, CancellationToken cancellationToken);
        Task<LookupListDto?> UpdateAsync(Guid id, CreateLookupListDto request, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
