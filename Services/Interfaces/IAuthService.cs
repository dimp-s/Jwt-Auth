using MyCourses.Models;
using MyCoursesApp.Commons;
using MyCoursesApp.Models.Dtos;

namespace MyCoursesApp.Services.Interfaces {
    public interface IAuthService {
        Task<ResultType<RegisterDto>> RegisterAsync(RegisterDto user);
        Task<ResultType<object>> LoginAsync(LoginDto user);
    }
}
