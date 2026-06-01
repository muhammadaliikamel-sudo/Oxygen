using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Oxygen.DTOs.Notification;
using Oxygen.Interfaces;

namespace Oxygen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _service;

        public NotificationController(INotificationService service)
        {_service = service;}
        //create 
        [HttpPost]
        public async Task<IActionResult> Create(CreateNotificationDto dto)
        {
            return Ok(await _service.CreateAsync(dto));
        }
        //get  
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserNotifications(Guid userId)
        {
            return Ok(await _service.GetUserNotificationsAsync(userId));
        }
        //read
        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkAsRead(Guid id)
        {
            return Ok(await _service.MarkAsReadAsync(id));
        }
    }
}