using Oxygen.DTOs.AuditLog;

namespace Oxygen.Interfaces
{
    public interface IAuditLogService
    {
        Task<List<AuditLogResponseDto>> GetAllAsync();
        Task<AuditLogResponseDto> GetByIdAsync(Guid id);
        Task CreateAsync(Guid? userId, string entityType,Guid? entityId,string action,string? oldValue,string? newValue,string? ipAddress);
    }
}