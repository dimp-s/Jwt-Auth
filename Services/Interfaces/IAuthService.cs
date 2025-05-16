using MyCourses.Models;
using MyCoursesApp.Models.Dtos;

namespace MyCoursesApp.Services.Interfaces {
    public interface IAuthService {
        Task<User> RegisterAsync(RegisterDto user);
        Task<string?> LoginAsync(LoginDto user);

        Task<RegisterDto> GetDashboardDataAsync(string id);
    }
}
