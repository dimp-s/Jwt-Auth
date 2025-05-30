using Microsoft.EntityFrameworkCore;
using MyCoursesApp.Commons;
using MyCoursesApp.Data;
using MyCoursesApp.Models;
using MyCoursesApp.Models.Dtos;
using MyCoursesApp.Services.Interfaces;

namespace MyCoursesApp.Services {
    public class CourseService: ICourseService {
        private readonly AppDbContext _context;

        public CourseService(AppDbContext context) {
            _context = context;
        }

        public async Task<ResultType> CreateAsync(CourseDto course) {
            var newCourse = new Course { Name = course.Name, CreditHours = course.CreditHours, Description = course.Description };
            _context.Courses.Add(newCourse);
            await _context.SaveChangesAsync();
            return ResultType.Ok("Course Created Successfully!");
        }

        public async Task<ResultType> DeleteAsync(int id) {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return ResultType.Fail("Course not found.");

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return ResultType.Ok("Course deleted.");
        }

        

        public async Task<ResultType<List<Course>>> GetAllAsync() {
            var courses = await _context.Courses.ToListAsync();
            return ResultType<List<Course>>.Ok(courses, "Courses retrieved.");
        }

        public async Task<ResultType<Course>> GetDetailsAsync(int? id) {
            if (id == null)
                return ResultType<Course>.Fail("Invalid course ID.");

            var course = await _context.Courses.FirstOrDefaultAsync(i => i.Id == id);
            return course != null
                ? ResultType<Course>.Ok(course, "Course found.")
                : ResultType<Course>.Fail("Course not found.");
        }

        public async Task<ResultType> UpdateAsync(int id, CourseDto course) {
            var updated = await _context.Courses.FindAsync(id);
            if (updated == null)
                return ResultType.Fail("Course not found.");

            updated.Name = course.Name;
            updated.CreditHours = course.CreditHours;
            updated.Description = course.Description;

            await _context.SaveChangesAsync();
            return ResultType.Ok("Course updated successfully.");

        }
        public bool CourseExists(int id) {
            return _context.Courses.Any(e => e.Id == id);
        }
    }


}

