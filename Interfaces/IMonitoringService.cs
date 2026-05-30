using Oxygen.DTOs;

namespace Oxygen.Interfaces
{
    public interface IMonitoringService
    {
        Task<IReadOnlyList<QueueStatusDto>> GetQueueStatusAsync(CancellationToken cancellationToken);
        Task<IReadOnlyList<BackgroundJobRunDto>> GetJobRunsAsync(int limit, CancellationToken cancellationToken);
        Task TriggerBackupAsync(CancellationToken cancellationToken);
        Task TriggerCleanupAsync(CancellationToken cancellationToken);
    }
}
