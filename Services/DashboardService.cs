using Microsoft.EntityFrameworkCore;
using Oxygen.Data;
using Oxygen.DTOs;
using Oxygen.Interfaces;

namespace Oxygen.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _dbContext;

        public DashboardService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<IReadOnlyList<DashboardMetricDto>> GetOverviewAsync(CancellationToken cancellationToken)
        {
            return GetMetricsAsync("overview.", cancellationToken);
        }

        public Task<IReadOnlyList<DashboardMetricDto>> GetCtaAsync(CancellationToken cancellationToken)
        {
            return GetMetricsAsync("cta.", cancellationToken);
        }

        public Task<IReadOnlyList<DashboardMetricDto>> GetCandidatesAsync(CancellationToken cancellationToken)
        {
            return GetMetricsAsync("candidates.", cancellationToken);
        }

        public async Task<IReadOnlyList<DashboardRealtimeItemDto>> GetRealtimeAsync(
            int limit,
            CancellationToken cancellationToken)
        {
            var candidateItems = await _dbContext.Candidates
                .OrderByDescending(item => item.CreatedAt)
                .Take(limit)
                .Select(item => new DashboardRealtimeItemDto
                {
                    Type = "candidate",
                    EntityId = item.Id,
                    Title = item.FullName,
                    CreatedAt = item.CreatedAt
                })
                .ToListAsync(cancellationToken);

            var visitorItems = await _dbContext.Visitors
                .OrderByDescending(item => item.CreatedAt)
                .Take(limit)
                .Select(item => new DashboardRealtimeItemDto
                {
                    Type = "visitor",
                    EntityId = item.Id,
                    Title = item.Name,
                    CreatedAt = item.CreatedAt
                })
                .ToListAsync(cancellationToken);

            var interactionItems = await _dbContext.Interactions
                .OrderByDescending(item => item.CreatedAt)
                .Take(limit)
                .Select(item => new DashboardRealtimeItemDto
                {
                    Type = "interaction",
                    EntityId = item.Id,
                    Title = item.Type,
                    CreatedAt = item.CreatedAt
                })
                .ToListAsync(cancellationToken);

            var ctaItems = await _dbContext.CTAs
                .OrderByDescending(item => item.CreatedAt)
                .Take(limit)
                .Select(item => new DashboardRealtimeItemDto
                {
                    Type = "cta",
                    EntityId = item.Id,
                    Title = item.Title,
                    CreatedAt = item.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return candidateItems
                .Concat(visitorItems)
                .Concat(interactionItems)
                .Concat(ctaItems)
                .OrderByDescending(item => item.CreatedAt)
                .Take(limit)
                .ToList();
        }

        public async Task<IReadOnlyList<DrilldownItemDto>> GetDrilldownAsync(
            string metric,
            int top,
            CancellationToken cancellationToken)
        {
            var normalizedMetric = metric.Trim().ToLowerInvariant();

            return normalizedMetric switch
            {
                "cta-status" => await _dbContext.CTAs
                    .GroupBy(item => item.Status)
                    .Select(group => new DrilldownItemDto
                    {
                        Label = group.Key,
                        Count = group.Count()
                    })
                    .OrderByDescending(item => item.Count)
                    .Take(top)
                    .ToListAsync(cancellationToken),
                "candidate-status" => await _dbContext.Candidates
                    .GroupBy(item => item.Status)
                    .Select(group => new DrilldownItemDto
                    {
                        Label = group.Key,
                        Count = group.Count()
                    })
                    .OrderByDescending(item => item.Count)
                    .Take(top)
                    .ToListAsync(cancellationToken),
                "visitors-by-category" => await _dbContext.Visitors
                    .GroupBy(item => item.category)
                    .Select(group => new DrilldownItemDto
                    {
                        Label = group.Key,
                        Count = group.Count()
                    })
                    .OrderByDescending(item => item.Count)
                    .Take(top)
                    .ToListAsync(cancellationToken),
                "visitors-by-source" => await _dbContext.Visitors
                    .GroupBy(item => item.Source)
                    .Select(group => new DrilldownItemDto
                    {
                        Label = group.Key,
                        Count = group.Count()
                    })
                    .OrderByDescending(item => item.Count)
                    .Take(top)
                    .ToListAsync(cancellationToken),
                _ => new List<DrilldownItemDto>()
            };
        }

        private async Task<IReadOnlyList<DashboardMetricDto>> GetMetricsAsync(
            string prefix,
            CancellationToken cancellationToken)
        {
            var metrics = await _dbContext.DashboardCache
                .Where(metric => metric.MetricName.StartsWith(prefix))
                .ToListAsync(cancellationToken);

            return metrics.Select(metric => new DashboardMetricDto
            {
                MetricName = metric.MetricName,
                MetricValue = metric.MetricValue,
                LastUpdated = metric.LastUpdated
            }).ToList();
        }
    }
}
