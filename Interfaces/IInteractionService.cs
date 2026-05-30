using Oxygen.DTOs.Interaction;

namespace Oxygen.Interfaces
{
    public interface IInteractionService
    {
        Task<InteractionResponseDto> CreateAsync(
            CreateInteractionDto dto);

        Task<List<InteractionResponseDto>> GetAllAsync();

        Task<InteractionResponseDto> GetByIdAsync(
            Guid id);

        Task<InteractionResponseDto> UpdateAsync(
            Guid id,
            UpdateInteractionDto dto);

        Task<bool> DeleteAsync(
            Guid id);
    }
}