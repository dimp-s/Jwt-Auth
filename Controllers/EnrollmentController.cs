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

        [Authorize(Roles = "Student")]
        [HttpGet("mycourses")]
        public async Task<IActionResult> GetMyCourses() {
            try {
                var studentId = _userContextService.GetCurrentStudentId();
                if (studentId == null)
                    return Unauthorized(new { message = "Invalid Student ID in token." });

                var courses = await _enrollmentService.GetAvailableCoursesAsync(studentId.Value);

                return Ok(new { message = "Courses retrieved", data = courses });
            }
            catch (Exception) {
                return StatusCode(500, new { message = "An error occurred while retrieving courses." });
            }
        }

        [Authorize(Roles = "Student")]
        [HttpPost("enroll/{courseId:int}")]
        public async Task<IActionResult> EnrollInCourse(int courseId) {
            try {
                var studentId = _userContextService.GetCurrentStudentId();
                if (studentId == null)
                    return Unauthorized(new { message = "Invalid Student ID in token." });

                var success = await _enrollmentService.EnrollStudentAsync(studentId.Value, courseId);

                if (!success)
                    return Conflict(new { message = "You are already enrolled in this course." });

                return Ok(new { message = "Enrollment successful." });
            }
            catch (Exception) {
                return StatusCode(500, new { message = "An error occurred while enrolling in the course." });
            }
        }

        [Authorize(Roles = "Student")]
        [HttpDelete("drop/{courseId:int}")]
        public async Task<IActionResult> DropCourse(int courseId) {
            try {
                var studentId = _userContextService.GetCurrentStudentId();
                if (studentId == null)
                    return Unauthorized(new { message = "Invalid Student Id in token" });

                var success = await _enrollmentService.DropCourseAsync(studentId.Value, courseId);

                if (!success)
                    return Conflict(new { message = "Failed to Drop course!" });

                return Ok(new { message = "Course dropped successfully." });
            }
            catch (Exception) {
                return StatusCode(500, new { message = "An error occurred while dropping the course." });
            }
        }

        [Authorize(Roles = "Student")]
        [HttpGet("search")]
        public async Task<IActionResult> SearchCourses([FromQuery] string query) {
            try {
                var studentId = _userContextService.GetCurrentStudentId();
                if (studentId == null)
                    return Unauthorized(new { message = "Invalid student ID in token." });

                var results = await _enrollmentService.SearchCoursesAsync(studentId.Value, query);
                return Ok(new { message = "Search results retrieved.", data = results });
            }
            catch {
                return StatusCode(500, new {message = "Search failed!." });
            }
        }

        //private int? GetCurrentStudentId() {
        //    var studentIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //    if (int.TryParse(studentIdClaim, out var studentId))
        //        return studentId;

        //    return null;
        //}

    }
}
