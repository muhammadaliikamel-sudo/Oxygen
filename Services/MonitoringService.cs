using Microsoft.EntityFrameworkCore;
using Oxygen.Data;
using Oxygen.DTOs;
using Oxygen.Interfaces;
using Oxygen.Models;

namespace Oxygen.Services
{
    public class MonitoringService : IMonitoringService
    {
        private readonly AppDbContext _dbContext;

        public MonitoringService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<QueueStatusDto>> GetQueueStatusAsync(CancellationToken cancellationToken)
        {
            var pendingExports = await _dbContext.ExportLogs
                .CountAsync(log => log.Status != "completed", cancellationToken);

            IReadOnlyList<QueueStatusDto> status = new List<QueueStatusDto>
            {
                new QueueStatusDto { QueueName = "exports", PendingCount = pendingExports },
                new QueueStatusDto { QueueName = "notifications", PendingCount = 0 },
                new QueueStatusDto { QueueName = "cleanup", PendingCount = 0 }
            };

            return status;
        }

        public async Task<IReadOnlyList<BackgroundJobRunDto>> GetJobRunsAsync(
            int limit,
            CancellationToken cancellationToken)
        {
            var jobs = await _dbContext.BackgroundJobRuns
                .OrderByDescending(job => job.StartedAt)
                .Take(limit)
                .ToListAsync(cancellationToken);

            return jobs.Select(job => new BackgroundJobRunDto
            {
                Id = job.Id,
                JobName = job.JobName,
                Status = job.Status,
                StartedAt = job.StartedAt,
                CompletedAt = job.CompletedAt,
                Details = job.Details
            }).ToList();
        }

        public async Task TriggerBackupAsync(CancellationToken cancellationToken)
        {
            var run = new BackgroundJobRun
            {
                Id = Guid.NewGuid(),
                JobName = "backup",
                Status = "completed",
                StartedAt = DateTime.UtcNow,
                CompletedAt = DateTime.UtcNow,
                Details = "Backup triggered via API."
            };

            _dbContext.BackgroundJobRuns.Add(run);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task TriggerCleanupAsync(CancellationToken cancellationToken)
        {
            var run = new BackgroundJobRun
            {
                Id = Guid.NewGuid(),
                JobName = "cleanup",
                Status = "completed",
                StartedAt = DateTime.UtcNow,
                CompletedAt = DateTime.UtcNow,
                Details = "Cleanup triggered via API."
            };

            _dbContext.BackgroundJobRuns.Add(run);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
