using Microsoft.EntityFrameworkCore;
using Oxygen.Data;
using Oxygen.DTOs.Interaction;
using Oxygen.Interfaces;
using Oxygen.Models;

namespace Oxygen.Services
{
    public class InteractionService : IInteractionService
    {
        private readonly AppDbContext _context;
        private readonly IAuditLogService _auditLogService;

        public InteractionService(AppDbContext context, IAuditLogService auditLogService)
        {
            _context = context;_auditLogService = auditLogService;
        }
        // Create 
        public async Task<InteractionResponseDto> CreateAsync(
            CreateInteractionDto dto)
        {
            var interaction = new Interaction
            {
                Id = Guid.NewGuid(),

                VisitorId = dto.VisitorId,

                Type = dto.Type,

                Notes = dto.Notes,

                DurationMinutes = dto.DurationMinutes,

                CreatedAt = DateTime.UtcNow,

                UpdatedAt = DateTime.UtcNow
            };

            _context.Interactions.Add(interaction);

            await _context.SaveChangesAsync();
            await _auditLogService.CreateAsync(null,"Interaction",interaction.Id,"CREATE",null,System.Text.Json.JsonSerializer.Serialize(interaction),null);

            return new InteractionResponseDto
            {
                Id = interaction.Id,

                VisitorId = interaction.VisitorId,

                Type = interaction.Type,

                Notes = interaction.Notes,

                DurationMinutes = interaction.DurationMinutes,

                CreatedBy = interaction.CreatedBy,

                CreatedAt = interaction.CreatedAt
            };
        }
        // Get
        public async Task<List<InteractionResponseDto>>
            GetAllAsync()
        {
            return await _context.Interactions
                .Select(i => new InteractionResponseDto
                {
                    Id = i.Id,

                    VisitorId = i.VisitorId,

                    Type = i.Type,

                    Notes = i.Notes,

                    DurationMinutes = i.DurationMinutes,

                    CreatedBy = i.CreatedBy,

                    CreatedAt = i.CreatedAt
                })
                .ToListAsync();
        }
        // Get By Id
        public async Task<InteractionResponseDto>
            GetByIdAsync(Guid id)
        {
            var interaction =
                await _context.Interactions
                .Where(i => i.Id == id)
                .Select(i => new InteractionResponseDto
                {
                    Id = i.Id,

                    VisitorId = i.VisitorId,

                    Type = i.Type,

                    Notes = i.Notes,

                    DurationMinutes = i.DurationMinutes,

                    CreatedBy = i.CreatedBy,

                    CreatedAt = i.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (interaction == null)
            {
                throw new Exception(
                    "Interaction not found");
            }

            return interaction;
        }
        // Update 
        public async Task<InteractionResponseDto>
            UpdateAsync(
                Guid id,
                UpdateInteractionDto dto)
        {
            var interaction =await _context.Interactions.FirstOrDefaultAsync(i => i.Id == id);
            var oldValue =System.Text.Json.JsonSerializer.Serialize(interaction);

            if (interaction == null)
            {
                throw new Exception("Interaction not found");
            }

            interaction.Type = dto.Type;

            interaction.Notes = dto.Notes;

            interaction.DurationMinutes =
                dto.DurationMinutes;

            interaction.UpdatedAt =
                DateTime.UtcNow;

            await _context.SaveChangesAsync();
            await _auditLogService.CreateAsync(null,"Interaction",interaction.Id,"UPDATE",oldValue,System.Text.Json.JsonSerializer.Serialize(interaction),null);

            return new InteractionResponseDto
            {
                Id = interaction.Id,

                VisitorId = interaction.VisitorId,

                Type = interaction.Type,

                Notes = interaction.Notes,

                DurationMinutes =interaction.DurationMinutes,

                CreatedBy = interaction.CreatedBy,

                CreatedAt = interaction.CreatedAt
            };
        }
        // Delete
        public async Task<bool> DeleteAsync(Guid id)
        {
            var interaction =await _context.Interactions.FirstOrDefaultAsync(i => i.Id == id);
            var oldValue =System.Text.Json.JsonSerializer.Serialize(interaction);

            if (interaction == null)
            {
                throw new Exception("Interaction not found");
            }

            _context.Interactions.Remove(interaction);

            await _context.SaveChangesAsync();
            await _auditLogService.CreateAsync(null,"Interaction",interaction.Id,"DELETE",oldValue,null,null);

            return true;
        }
    }
}