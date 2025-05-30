using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyCourses.Models;
using MyCoursesApp.Commons;
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

        public async Task<ResultType<object>> LoginAsync(LoginDto request) {
            var user = await _context.Users.Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
                return ResultType<object>.Fail("Invalid email or password.");

            var passwordValid = new PasswordHasher<User>()
                .VerifyHashedPassword(user, user.PasswordHash, request.PasswordHash);

            if (passwordValid == PasswordVerificationResult.Failed)
                return ResultType<object>.Fail("Invalid email or password.");

            var token = CreateToken(user);
            return ResultType<object>.Ok(new { token } , "Login successful.");
        }

        public async Task<ResultType<RegisterDto>> RegisterAsync(RegisterDto request) {

            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                return ResultType<RegisterDto>.Fail("Email is already registered.");

            var defaultRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Student");
            if (defaultRole == null)
                return ResultType<RegisterDto>.Fail("Default role not found.");

            var user = new User {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = new PasswordHasher<User>().HashPassword(null!, request.PasswordHash),
                ProfileImage = request.ProfileImage,
                RoleId = defaultRole.Id
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var responseDto = new RegisterDto {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfileImage = user.ProfileImage
            };

            return ResultType<RegisterDto>.Ok(responseDto, "Registration successful.");
        }

        private string CreateToken(User user) {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FirstName),
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
