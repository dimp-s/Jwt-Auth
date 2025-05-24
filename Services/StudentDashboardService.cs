using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyCoursesApp.Data;
using MyCoursesApp.Models.Dtos;
using MyCoursesApp.Services.Interfaces;

namespace MyCoursesApp.Services {
    public class StudentDashboardService : IStudentDashboardService {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _contextAccessor;
        public StudentDashboardService(AppDbContext context, IWebHostEnvironment env, IHttpContextAccessor contextAccessor) {
            _context = context;
            _env = env;
            _contextAccessor = contextAccessor;
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

        public async Task<string> UploadProfilePhotoAsync(int? studentId, IFormFile profileImage) {
            if (profileImage == null || profileImage.Length == 0)
                throw new ArgumentException("File is Invalid!");

            var student = await _context.Users.FindAsync(studentId);
            if (student == null) throw new Exception("Student not found.");

            // Use ContentRootPath + wwwroot
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "profiles");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(profileImage.FileName)}";
            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create)) {
                await profileImage.CopyToAsync(stream);
            }

            // Save relative public path (as served by UseStaticFiles)
            var relativePath = $"/images/profiles/{fileName}";
            // full URL for frontend
            var request = _contextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";

            var fullImageUrl = $"{baseUrl}{relativePath}";

            student.ProfileImage = relativePath;
            await _context.SaveChangesAsync();

            return fullImageUrl;
        }
    }
}
