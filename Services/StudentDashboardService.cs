using Microsoft.EntityFrameworkCore;
using MyCoursesApp.Data;
using MyCoursesApp.Models.Dtos;
using MyCoursesApp.Services.Interfaces;

namespace MyCoursesApp.Services {
    public class StudentDashboardService : IStudentDashboardService {
        private readonly AppDbContext _context;
         public StudentDashboardService(AppDbContext context) {
            _context = context;
        }

        public async Task<StudentDashboardDto> GetDashboardDataAsync(int? studentId) {
            var student = await _context.Users
           .Include(s => s.Enrollments)
               .ThenInclude(e => e.Course)
           .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student == null) return null;

            return new StudentDashboardDto {
                FirstName = student.FirstName,

                LastName = student.LastName,
                ProfileImage = student.ProfileImage,
                EnrolledCourses = student.Enrollments.Select(e => new StudentDashboardDto.CourseInfo {
                    CourseId = e.CourseId,
                    Name = e.Course.Name,
                    CreditHours = e.Course.CreditHours,
                    Description = e.Course.Description,
                    EnrolledOn = e.EnrolledOn
                }).ToList()
            };
        }

        Task<string> IStudentDashboardService.UploadProfilePhotoAsync(string studentId, IFormFile profileImage) {
            throw new NotImplementedException();
        }
    }
}
