using Microsoft.EntityFrameworkCore;
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

        public async Task CreateAsync(CourseDto course) {
            var newCourse = new Course { Name = course.Name, CreditHours = course.CreditHours, Description = course.Description };
            _context.Courses.Add(newCourse);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id) {
            var course = await _context.Courses.FindAsync(id);
            if (course != null) {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
            }
            else {
                throw new Exception("Course Not Found!");
            }

        }

        public async Task<List<Course>> GetAllAsync() {
            return await _context.Courses.ToListAsync();
        }

        public async Task<Course?> GetDetailsAsync(int? id) {
            return await _context.Courses.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task UpdateAsync(int id, CourseDto course) {
            var updated = await _context.Courses.FindAsync(id);
            if (updated != null) {
                updated.Name = course.Name;
                updated.CreditHours = course.CreditHours;
                updated.Description = course.Description;
                await _context.SaveChangesAsync();
            }
        }
        public bool CourseExists(int id) {
            return _context.Courses.Any(e => e.Id == id);
        }
    }


}

