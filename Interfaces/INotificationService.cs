using Oxygen.DTOs.Notification;

namespace Oxygen.Interfaces
{
    public interface INotificationService
    {
      Task<NotificationResponseDto> CreateAsync(CreateNotificationDto dto);
      Task<List<NotificationResponseDto>> GetUserNotificationsAsync( Guid userId);
      Task<bool> MarkAsReadAsync(Guid id);
        
    }
}
