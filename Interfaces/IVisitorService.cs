using Oxygen.DTOs.Visitor;

namespace Oxygen.Interfaces
{
    public interface IVisitorService
    {
        Task<VisitorResponseDto> CreateAsync(
            CreateVisitorDto dto);

        Task<List<VisitorResponseDto>> GetAllAsync();

        Task<VisitorResponseDto> GetByIdAsync(
            Guid id);

        Task<VisitorResponseDto> UpdateAsync(
            Guid id,
            UpdateVisitorDto dto);

        Task<bool> DeleteAsync(
            Guid id);
    }
}
    
