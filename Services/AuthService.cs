using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyCourses.Models;
using MyCoursesApp.Data;
using MyCoursesApp.Models.Dtos;
using MyCoursesApp.Services.Interfaces;

namespace MyCoursesApp.Services {
    public class AuthService : IAuthService {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration) {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string?> LoginAsync(LoginDto request) {
            var user = await _context.Users.
                Include(u => u.Role).
                FirstOrDefaultAsync(u  => u.Email == request.Email);
            if (user == null) {
                return null;
            }
            if (new PasswordHasher<User>().
                VerifyHashedPassword(user, user.PasswordHash, request.PasswordHash)
                == PasswordVerificationResult.Failed)
                return null;
            return CreateToken(user);
        }

        public async Task<RegisterDto> RegisterAsync(RegisterDto request) {

            if (await _context.Users.AnyAsync(u => u.Email == request.Email)) {
                return null;
            }

            var defaultRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Student");
            if (defaultRole == null) throw new Exception("Default role not found");


            var user = new User();

            var hashedPassword = new PasswordHasher<User>()
            .HashPassword(user, request.PasswordHash);
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.PasswordHash = hashedPassword;
            user.ProfileImage = request.ProfileImage;
            user.RoleId = defaultRole.Id;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new RegisterDto {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfileImage = user.ProfileImage
            };
        }

        private string CreateToken(User user) {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:Token")!
                ));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("AppSettings:Issuer"),
                audience: _configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
