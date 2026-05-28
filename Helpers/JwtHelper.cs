using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Oxygen.Models;

namespace Oxygen.Helpers
{
    public class JwtHelper
    {
        public static string GenerateToken(
            User user,
            IConfiguration configuration)
        {
            // Claims
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };
            // Secret Key
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    configuration["Jwt:Key"]));
            // Signing Credentials  
            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);
            // Expiration
            var expires =
                DateTime.Now.AddMinutes(
                    Convert.ToDouble(
                        configuration["Jwt:DurationInMinutes"]));
            // Create Token
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );
            //Return Token
            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}