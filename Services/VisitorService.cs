using Microsoft.EntityFrameworkCore;
using Oxygen.Data;
using Oxygen.DTOs.Visitor;
using Oxygen.Interfaces;
using Oxygen.Models;

namespace Oxygen.Services
{
    public class VisitorService : IVisitorService
    {
        private readonly AppDbContext _context;

        public VisitorService(AppDbContext context)
        {
            _context = context;
        }
        // Create 
        public async Task<VisitorResponseDto> CreateAsync(
            CreateVisitorDto dto)
        {
            var visitor = new Visitor
            {
                Id = Guid.NewGuid(),

                FullName = dto.FullName,

                Email = dto.Email,

                Phone = dto.Phone,

                Category = dto.Category,

                Source = dto.Source,

                Language = dto.Language,

                CreatedAt = DateTime.UtcNow,

                UpdatedAt = DateTime.UtcNow,

                IsDeleted = false
            };

            _context.Visitors.Add(visitor);

            await _context.SaveChangesAsync();

            return new VisitorResponseDto
            {
                Id = visitor.Id,

                FullName = visitor.FullName,

                Email = visitor.Email,

                Phone = visitor.Phone,

                Category = visitor.Category,

                Source = visitor.Source,

                Language = visitor.Language,

                CreatedAt = visitor.CreatedAt
            };
        }
        // Get 
        public async Task<List<VisitorResponseDto>>
            GetAllAsync()
        {
            var visitors = await _context.Visitors
                .Where(v => !v.IsDeleted)
                .Select(v => new VisitorResponseDto
                {
                    Id = v.Id,

                    FullName = v.FullName,

                    Email = v.Email,

                    Phone = v.Phone,

                    Category = v.Category,

                    Source = v.Source,

                    Language = v.Language,

                    CreatedAt = v.CreatedAt
                })
                .ToListAsync();

            return visitors;
        }
        // Get By Id
        public async Task<VisitorResponseDto>
            GetByIdAsync(Guid id)
        {
            var visitor = await _context.Visitors
                .Where(v => !v.IsDeleted && v.Id == id)
                .Select(v => new VisitorResponseDto
                {
                    Id = v.Id,

                    FullName = v.FullName,

                    Email = v.Email,

                    Phone = v.Phone,

                    Category = v.Category,

                    Source = v.Source,

                    Language = v.Language,

                    CreatedAt = v.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (visitor == null)
            {
                throw new Exception("Visitor not found");
            }

            return visitor;
        }
        // Update 
        public async Task<VisitorResponseDto>
            UpdateAsync(
                Guid id,
                UpdateVisitorDto dto)
        {
            var visitor = await _context.Visitors
                .FirstOrDefaultAsync(v =>
                    v.Id == id &&
                    !v.IsDeleted);

            if (visitor == null)
            {
                throw new Exception("Visitor not found");
            }

            visitor.FullName = dto.FullName;

            visitor.Email = dto.Email;

            visitor.Phone = dto.Phone;

            visitor.Category = dto.Category;

            visitor.Source = dto.Source;

            visitor.Language = dto.Language;

            visitor.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new VisitorResponseDto
            {
                Id = visitor.Id,

                FullName = visitor.FullName,

                Email = visitor.Email,

                Phone = visitor.Phone,

                Category = visitor.Category,

                Source = visitor.Source,

                Language = visitor.Language,

                CreatedAt = visitor.CreatedAt
            };
        }
        // Delete 
        public async Task<bool> DeleteAsync(Guid id)
        {
            var visitor = await _context.Visitors
                .FirstOrDefaultAsync(v =>
                    v.Id == id &&
                    !v.IsDeleted);

            if (visitor == null)
            {
                throw new Exception("Visitor not found");
            }

            visitor.IsDeleted = true;

            visitor.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}