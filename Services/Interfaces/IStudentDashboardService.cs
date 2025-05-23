using MyCoursesApp.Models.Dtos;

namespace MyCoursesApp.Services.Interfaces {
    public interface IStudentDashboardService {
        Task<StudentDashboardDto> GetDashboardDataAsync(int? studentId);
        Task<String> UploadProfilePhotoAsync(int? studentId, IFormFile profileImage);
    }
}
