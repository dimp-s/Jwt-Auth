using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyCoursesApp.Services.Interfaces;

namespace MyCoursesApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase {
        private readonly IEnrollmentService _enrollmentService;
        private readonly IUserContextService _userContextService;

        public EnrollmentController(IEnrollmentService enrollmentService, IUserContextService userContextService) {
            _enrollmentService = enrollmentService;
            _userContextService = userContextService;
        }

        [Authorize]
        [HttpGet("mycourses")]
        public async Task<IActionResult> GetMyCourses() {
            var studentId = _userContextService.GetCurrentStudentId();
            if (studentId == null)
                return Unauthorized(new { message = "Invalid Student ID in token." });

            var courses = await _enrollmentService.GetAvailableCoursesAsync(studentId.Value);

            return Ok(new { message = "Courses retrieved", data = courses });
        }

        [Authorize]
        [HttpPost("enroll/{courseId:int}")]
        public async Task<IActionResult> EnrollInCourse(int courseId) {
            var studentId = _userContextService.GetCurrentStudentId();
            if (studentId == null)
                return Unauthorized(new { message = "Invalid Student ID in token." });

            Console.WriteLine(studentId.Value);
            var success = await _enrollmentService.EnrollStudentAsync(studentId.Value, courseId);

            if (!success)
                return Conflict(new { message = "You are already enrolled in this course." });

            return Ok(new { message = "Enrollment successful." });
        }

        //private int? GetCurrentStudentId() {
        //    var studentIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //    if (int.TryParse(studentIdClaim, out var studentId))
        //        return studentId;

        //    return null;
        //}

    }
}
