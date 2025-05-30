using MyCoursesApp.Commons;
using MyCoursesApp.Models.Dtos;

namespace MyCoursesApp.Services.Interfaces {
    public interface IEnrollmentService {
        Task<ResultType<List<EnrollmentDto>>> GetAvailableCoursesAsync(int studentId);
        Task<ResultType<string>> EnrollStudentAsync(int studentId, int courseId);
        Task<ResultType<string>> DropCourseAsync(int studentId, int courseId);
        Task<ResultType<List<EnrollmentDto>>> SearchCoursesAsync(int studentId, string query);
    }
}
