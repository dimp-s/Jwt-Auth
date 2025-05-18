using MyCoursesApp.Models;
using MyCoursesApp.Models.Dtos;

namespace MyCoursesApp.Services.Interfaces {
    public interface ICourseService {
        Task<List<Course>> GetAllAsync();
        Task<Course> GetDetailsAsync(int? id);
        Task CreateAsync(CourseDto course);
        Task UpdateAsync(int id, CourseDto course);

        Task DeleteAsync(int id);
        bool CourseExists(int id);
    }
}
