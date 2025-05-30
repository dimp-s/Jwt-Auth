using MyCoursesApp.Commons;
using MyCoursesApp.Models;
using MyCoursesApp.Models.Dtos;

namespace MyCoursesApp.Services.Interfaces {
    public interface ICourseService {
        Task<ResultType<List<Course>>> GetAllAsync();
        Task<ResultType<Course>> GetDetailsAsync(int? id);
        Task<ResultType> CreateAsync(CourseDto course);
        Task<ResultType> UpdateAsync(int id, CourseDto course);

        Task<ResultType> DeleteAsync(int id);
        bool CourseExists(int id);
    }
}
