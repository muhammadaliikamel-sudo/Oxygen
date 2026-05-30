using Oxygen.DTOs;

namespace Oxygen.Interfaces
{
    public interface IExportService
    {
        Task<ExportFileResultDto> ExportVisitorsAsync(string format, Guid? userId, CancellationToken cancellationToken);
        Task<ExportFileResultDto> ExportCtasAsync(string format, Guid? userId, CancellationToken cancellationToken);
        Task<ExportFileResultDto> ExportFinalAsync(Guid? userId, CancellationToken cancellationToken);
    }
}
