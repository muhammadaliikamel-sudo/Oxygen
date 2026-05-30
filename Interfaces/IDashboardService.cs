using Oxygen.DTOs;

namespace Oxygen.Interfaces
{
    public interface IDashboardService
    {
        Task<IReadOnlyList<DashboardMetricDto>> GetOverviewAsync(CancellationToken cancellationToken);
        Task<IReadOnlyList<DashboardMetricDto>> GetCtaAsync(CancellationToken cancellationToken);
        Task<IReadOnlyList<DashboardMetricDto>> GetCandidatesAsync(CancellationToken cancellationToken);
        Task<IReadOnlyList<DashboardRealtimeItemDto>> GetRealtimeAsync(int limit, CancellationToken cancellationToken);
        Task<IReadOnlyList<DrilldownItemDto>> GetDrilldownAsync(string metric, int top, CancellationToken cancellationToken);
    }
}
