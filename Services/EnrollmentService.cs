using Microsoft.EntityFrameworkCore;
using MyCoursesApp.Data;
using MyCoursesApp.Models;
using MyCoursesApp.Models.Dtos;
using MyCoursesApp.Services.Interfaces;

namespace MyCoursesApp.Services {
    public class EnrollmentService : IEnrollmentService {
        private readonly AppDbContext _context;

        public EnrollmentService(AppDbContext context) {
            _context = context;
        }
        public async Task<bool> EnrollStudentAsync(int studentId, int courseId) {
            var alreadyEnrolled = await _context.Enrollments
               .AnyAsync(e => e.UserId == studentId && e.CourseId == courseId);

            if (alreadyEnrolled)
                return false;

            var enrollment = new Enrollment {
                UserId = studentId,
                CourseId = courseId,
                EnrolledOn = DateTime.UtcNow
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DropCourseAsync(int studentId, int courseId) {
            var result = await _context.Enrollments.FirstOrDefaultAsync(e => e.UserId ==  studentId && e.CourseId == courseId);
            if(result == null) return false;
            _context.Enrollments.Remove(result);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<EnrollmentDto>> GetAvailableCoursesAsync(int studentId) {
            var enrolledCourseIds = await _context.Enrollments
            .Where(e => e.UserId == studentId)
            .Select(e => e.CourseId)
            .ToListAsync();

            var allCourses = await _context.Courses.ToListAsync();

            return allCourses.Select(course => new EnrollmentDto {
                CourseId = course.Id,
                CourseName = course.Name,
                CreditHours = course.CreditHours,
                Description = course.Description,
                IsEnrolled = enrolledCourseIds.Contains(course.Id)
            }).ToList();
        }

        public async Task<List<EnrollmentDto>> SearchCoursesAsync(int studentId, string query) {
            var lowerQuery = query.ToLower();
            var enrolledCoursesIds = await _context.Enrollments.
                Where(e => e.UserId == studentId).
                Select(e => e.CourseId)
                .ToListAsync();
            var matchingCourses = await _context.Courses
                .Where(c =>
                c.Name.ToLower().Contains(lowerQuery) ||
                c.Description.ToLower().Contains(lowerQuery))
                .ToListAsync();

            return matchingCourses.Select(course => new EnrollmentDto {
                CourseId = course.Id,
                CourseName = course.Name,
                CreditHours = course.CreditHours,
                Description = course.Description,
                IsEnrolled = enrolledCoursesIds.Contains(course.Id)
            }).ToList();
        }
    }
}
