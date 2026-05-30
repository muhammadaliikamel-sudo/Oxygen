using Microsoft.EntityFrameworkCore;
using Oxygen.Data;
using Oxygen.DTOs;
using Oxygen.Interfaces;
using Oxygen.Models;

namespace Oxygen.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly AppDbContext _dbContext;

        public SettingsService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<SettingDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var settings = await _dbContext.Settings
                .OrderBy(setting => setting.SettingKey)
                .ToListAsync(cancellationToken);

            return settings.Select(setting => new SettingDto
            {
                SettingKey = setting.SettingKey,
                SettingValue = setting.SettingValue
            }).ToList();
        }

        public async Task<IReadOnlyList<SettingDto>> UpdateAsync(
            UpdateSettingsRequestDto request,
            CancellationToken cancellationToken)
        {
            foreach (var update in request.Settings)
            {
                var normalizedKey = update.SettingKey.Trim();
                var existing = await _dbContext.Settings
                    .FirstOrDefaultAsync(setting => setting.SettingKey == normalizedKey, cancellationToken);

                if (existing is null)
                {
                    _dbContext.Settings.Add(new Setting
                    {
                        Id = Guid.NewGuid(),
                        SettingKey = normalizedKey,
                        SettingValue = update.SettingValue.Trim()
                    });
                }
                else
                {
                    existing.SettingValue = update.SettingValue.Trim();
                }
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            return await GetAllAsync(cancellationToken);
        }
    }
}
