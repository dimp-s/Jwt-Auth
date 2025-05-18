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
                IsEnrolled = enrolledCourseIds.Contains(course.Id)
            }).ToList();
        }
    }
}
