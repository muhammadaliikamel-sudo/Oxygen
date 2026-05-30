using Oxygen.DTOs;

namespace Oxygen.Interfaces
{
    public interface ISettingsService
    {
        Task<IReadOnlyList<SettingDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<IReadOnlyList<SettingDto>> UpdateAsync(UpdateSettingsRequestDto request, CancellationToken cancellationToken);
    }
}
