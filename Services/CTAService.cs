using Microsoft.EntityFrameworkCore;
using Oxygen.Data;
using Oxygen.DTOs.CTA;
using Oxygen.Interfaces;
using Oxygen.Models;

namespace Oxygen.Services
{
    public class CTAService : ICTAService
    {
        private readonly AppDbContext _context;

        public CTAService(AppDbContext context)
        {
            _context = context;
        }
        // CREATE
        public async Task<CTAResponseDto> CreateAsync(CreateCTADto dto)
        {
            var interactionExists = await _context.Interactions
                .AnyAsync(i => i.Id == dto.InteractionId && !i.IsDeleted);

            if (!interactionExists)
            {
                throw new Exception("The specified Interaction does not exist.");
            }

            var ownerExists = await _context.Users
                .AnyAsync(u => u.Id == dto.OwnerId && !u.IsDeleted);

            if (!ownerExists)
            {
                throw new Exception("The specified Owner does not exist.");
            }

            var cta = new CTA
            {
                Id = Guid.NewGuid(),
                InteractionId = dto.InteractionId,
                OwnerId = dto.OwnerId,
                Type = dto.Type,
                Status = dto.Status,
                DueDate = dto.DueDate,
                SLAState = CalculateSlaState(dto.Status, dto.DueDate),
            };
            _context.CTAs.Add(cta);
            await _context.SaveChangesAsync();
            return MapToResponse(cta);
        }
        // GET 
        public async Task<List<CTAResponseDto>> GetAllAsync()
        {
            return await _context.CTAs.Where(c => !c.IsDeleted).Select(c => MapToResponse(c)).ToListAsync();
        }
        // get by id
        public async Task<CTAResponseDto> GetByIdAsync(Guid id)
        {
            var cta = await _context.CTAs.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
            if (cta == null)
                throw new Exception("CTA not found");
            return MapToResponse(cta);
        }
        // UPDATE
        public async Task<CTAResponseDto> UpdateAsync(Guid id, UpdateCTADto dto)
        {
            var cta = await _context.CTAs.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
            if (cta == null)throw new Exception("CTA not found");
            cta.Type = dto.Type;
            cta.Status = dto.Status;
            if (dto.DueDate.HasValue)cta.DueDate = dto.DueDate.Value;
            cta.UpdatedAt = DateTime.UtcNow;
            cta.SLAState = CalculateSlaState(cta.Status, cta.DueDate);
            await _context.SaveChangesAsync();
            return MapToResponse(cta);
        }
        // DELETE
        public async Task<bool> DeleteAsync(Guid id)
        {
            var cta = await _context.CTAs.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
            if (cta == null)throw new Exception("CTA not found");
            cta.IsDeleted = true;
            cta.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        // MAPPER
        private static CTAResponseDto MapToResponse(CTA cta)
        {
            return new CTAResponseDto
            {
                Id = cta.Id,
                InteractionId = cta.InteractionId,
                OwnerId = cta.OwnerId,
                Type = cta.Type,
                Status = cta.Status,
                DueDate = cta.DueDate,
                SLAState = cta.SLAState,
                CreatedAt = cta.CreatedAt
            };
        }
        private string CalculateSlaState(string status, DateTime? dueDate)
        {
            if (status?.ToLower() == "completed")
                return "completed";

            if (dueDate.HasValue && DateTime.UtcNow > dueDate.Value)
                return "overdue";

            return "on_time";
        }
    }
}