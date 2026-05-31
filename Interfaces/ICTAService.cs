using Oxygen.DTOs.CTA;

namespace Oxygen.Interfaces
{
    public interface ICTAService
    {
        Task<CTAResponseDto> CreateAsync(CreateCTADto dto);
        Task<List<CTAResponseDto>> GetAllAsync();
        Task<CTAResponseDto> GetByIdAsync( Guid id);
        Task<CTAResponseDto> UpdateAsync(Guid id, UpdateCTADto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}   