using MyCoursesApp.Models.Dtos;

namespace MyCoursesApp.Services.Interfaces {
    public interface IEnrollmentService {
        Task<List<EnrollmentDto>> GetAvailableCoursesAsync(int studentId);
        Task<bool> EnrollStudentAsync(int studentId, int courseId);
        Task<bool> DropCourseAsync(int studentId, int courseId);
        Task<List<EnrollmentDto>> SearchCoursesAsync(int studentId, string query);
    }
}
