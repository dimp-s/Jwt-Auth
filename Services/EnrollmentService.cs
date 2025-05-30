using Microsoft.EntityFrameworkCore;
using MyCoursesApp.Commons;
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
        public async Task<ResultType<string>> EnrollStudentAsync(int studentId, int courseId) {
            var alreadyEnrolled = await _context.Enrollments
               .AnyAsync(e => e.UserId == studentId && e.CourseId == courseId);

            if (alreadyEnrolled)
                return ResultType<string>.Fail("Student is already enrolled in this course.");

            var enrollment = new Enrollment {
                UserId = studentId,
                CourseId = courseId,
                EnrolledOn = DateTime.UtcNow
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return ResultType<string>.Ok("Enrollment successful.");
        }
        public async Task<ResultType<string>> DropCourseAsync(int studentId, int courseId) {
            var result = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.UserId == studentId && e.CourseId == courseId);

            if (result == null)
                return ResultType<string>.Fail("Enrollment not found.");

            _context.Enrollments.Remove(result);
            await _context.SaveChangesAsync();

            return ResultType<string>.Ok("Course dropped successfully.");
        }
        public async Task<ResultType<List<EnrollmentDto>>> GetAvailableCoursesAsync(int studentId) {
            var enrolledCourseIds = await _context.Enrollments
                .Where(e => e.UserId == studentId)
                .Select(e => e.CourseId)
                .ToListAsync();

            var allCourses = await _context.Courses.ToListAsync();

            var result = allCourses.Select(course => new EnrollmentDto {
                CourseId = course.Id,
                CourseName = course.Name,
                CreditHours = course.CreditHours,
                Description = course.Description,
                IsEnrolled = enrolledCourseIds.Contains(course.Id)
            }).ToList();

            return ResultType<List<EnrollmentDto>>.Ok(result, "Courses retrieved!");
        }

        public async Task<ResultType<List<EnrollmentDto>>> SearchCoursesAsync(int studentId, string query) {
            var lowerQuery = query.ToLower();
            var enrolledCoursesIds = await _context.Enrollments
                .Where(e => e.UserId == studentId)
                .Select(e => e.CourseId)
                .ToListAsync();

            var matchingCourses = await _context.Courses
                .Where(c =>
                    c.Name.ToLower().Contains(lowerQuery) ||
                    c.Description.ToLower().Contains(lowerQuery))
                .ToListAsync();

            var result = matchingCourses.Select(course => new EnrollmentDto {
                CourseId = course.Id,
                CourseName = course.Name,
                CreditHours = course.CreditHours,
                Description = course.Description,
                IsEnrolled = enrolledCoursesIds.Contains(course.Id)
            }).ToList();

            return ResultType<List<EnrollmentDto>>.Ok(result, "Search results returned.");
        }
    }
}
