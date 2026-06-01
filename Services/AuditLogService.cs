using Microsoft.EntityFrameworkCore;
using Oxygen.Data;
using Oxygen.DTOs.AuditLog;
using Oxygen.Interfaces;
using Oxygen.Models;
namespace Oxygen.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly AppDbContext _context;
        public AuditLogService(AppDbContext context) { _context = context; }
        public async Task CreateAsync(Guid? userId, string entityType, Guid? entityId, string action, string? oldValue, string? newValue, string? ipAddress)
        {
            var audit = new AuditLog
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                EntityType = entityType,
                EntityId = entityId,
                Action = action,
                OldValue = oldValue,
                NewValue = newValue,
                IpAddress = ipAddress,
                CreatedAt = DateTime.UtcNow
            };
            _context.AuditLogs.Add(audit);
            await _context.SaveChangesAsync();
        }
        public async Task<List<AuditLogResponseDto>> GetAllAsync()
        {
            return await _context.AuditLogs.OrderByDescending(a => a.CreatedAt).Select(a => new AuditLogResponseDto
            {
                Id = a.Id,
                UserId = a.UserId,
                EntityType = a.EntityType,
                EntityId = a.EntityId,
                Action = a.Action,
                OldValue = a.OldValue,
                NewValue = a.NewValue,
                IpAddress = a.IpAddress,
                CreatedAt = a.CreatedAt
            })
                .ToListAsync();
        }
        public async Task<AuditLogResponseDto> GetByIdAsync(Guid id)
        {
            var audit = await _context.AuditLogs.FirstOrDefaultAsync(a => a.Id == id);
            if (audit == null) throw new Exception("Audit Log not found");
            return new AuditLogResponseDto
            {
                Id = audit.Id,
                UserId = audit.UserId,
                EntityType = audit.EntityType,
                EntityId = audit.EntityId,
                Action = audit.Action,
                OldValue = audit.OldValue,
                NewValue = audit.NewValue,
                IpAddress = audit.IpAddress,
                CreatedAt = audit.CreatedAt
            };
        }
    }
}
