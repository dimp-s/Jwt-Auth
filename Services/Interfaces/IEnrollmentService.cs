using MyCoursesApp.Models.Dtos;

namespace MyCoursesApp.Services.Interfaces {
    public interface IEnrollmentService {
        Task<List<EnrollmentDto>> GetAvailableCoursesAsync(int studentId);
        Task<bool> EnrollStudentAsync(int studentId, int courseId);
    }
}
