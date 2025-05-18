using MyCourses.Models;
using MyCoursesApp.Models.Dtos;

namespace MyCoursesApp.Services.Interfaces {
    public interface IAuthService {
        Task<RegisterDto> RegisterAsync(RegisterDto user);
        Task<string?> LoginAsync(LoginDto user);
    }
}
