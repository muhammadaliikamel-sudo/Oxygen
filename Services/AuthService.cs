using Microsoft.EntityFrameworkCore;
using Oxygen.Data;
using Oxygen.DTOs.Auth;
using Oxygen.Helpers;
using Oxygen.Interfaces;
using Oxygen.Models;

namespace Oxygen.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;

        private readonly IConfiguration _configuration;

        public AuthService(
            AppDbContext context,
            IConfiguration configuration)
        {
            _context = context;

            _configuration = configuration;
        }
        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (existingUser != null)
            {
                throw new Exception("Email already exists");
            }

            var passwordHash =
                PasswordHelper.HashPassword(dto.Password);
            var user = new User
            {
                Id = Guid.NewGuid(),

                FullName = dto.FullName,

                Email = dto.Email,

                PasswordHash = passwordHash,

                IsActive = true,

                CreatedAt = DateTime.UtcNow,

                UpdatedAt = DateTime.UtcNow
            };


            _context.Users.Add(user);

            await _context.SaveChangesAsync();


            var token =
                JwtHelper.GenerateToken(
                    user,
                    _configuration);

            return token;
        }


        public async Task<string> LoginAsync(LoginDto dto)
        {

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (user == null)
            {
                throw new Exception("Invalid email or password");
            }


            var isPasswordValid =
                PasswordHelper.VerifyPassword(
                    dto.Password,
                    user.PasswordHash);

            if (!isPasswordValid)
            {
                throw new Exception("Invalid email or password");
            }


            user.LastLogin = DateTime.UtcNow;

            await _context.SaveChangesAsync();


            var token =
                JwtHelper.GenerateToken(
                    user,
                    _configuration);

            return token;
        }
    }
}