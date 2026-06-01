using Microsoft.EntityFrameworkCore;
using Oxygen.Data;
using Oxygen.DTOs.Notification;
using Oxygen.Interfaces;
using Oxygen.Models;

namespace Oxygen.Services
{
    public class NotificationService : INotificationService
    {
        private readonly AppDbContext _context;
        public NotificationService(AppDbContext context)
        {_context = context;}
        public async Task<NotificationResponseDto> CreateAsync(CreateNotificationDto dto)
        {
            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                UserId = dto.UserId,
                Title = dto.Title,
                Message = dto.Message,
                Type = dto.Type,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            return Map(notification);
        }

        public async Task<List<NotificationResponseDto>>
            GetUserNotificationsAsync(Guid userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => Map(n))
                .ToListAsync();
        }

        public async Task<bool> MarkAsReadAsync(Guid id)
        {
            var notification =await _context.Notifications.FirstOrDefaultAsync(n => n.Id == id);
            if (notification == null)
            throw new Exception("Notification not found");
            notification.IsRead = true;
            await _context.SaveChangesAsync();
            return true;
        }

        private static NotificationResponseDto Map(Notification notification)
        {
            return new NotificationResponseDto
            {
                Id = notification.Id,
                UserId = notification.UserId,
                Title = notification.Title,
                Message = notification.Message,
                Type = notification.Type,
                IsRead = notification.IsRead,
                CreatedAt = notification.CreatedAt
            };
        }
    }
}