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
            var studentId = _userContextService.GetCurrentStudentId();
            if (studentId == null)
                return Unauthorized(new { message = "Invalid Student ID in token." });

            var result = await _enrollmentService.GetAvailableCoursesAsync(studentId.Value);
            if (!result.Success)
                return StatusCode(500, new { message = result.Message });

            return Ok(new { message = result.Message, data = result.Data });
        }

        [Authorize(Roles = "Student")]
        [HttpPost("enroll/{courseId:int}")]
        public async Task<IActionResult> EnrollInCourse(int courseId) {
            var studentId = _userContextService.GetCurrentStudentId();
            if (studentId == null)
                return Unauthorized(new { message = "Invalid Student ID in token." });

            var result = await _enrollmentService.EnrollStudentAsync(studentId.Value, courseId);
            if (!result.Success)
                return Conflict(new { message = result.Message });

            return Ok(new { message = result.Message });
        }

        [Authorize(Roles = "Student")]
        [HttpDelete("drop/{courseId:int}")]
        public async Task<IActionResult> DropCourse(int courseId) {
            var studentId = _userContextService.GetCurrentStudentId();
            if (studentId == null)
                return Unauthorized(new { message = "Invalid Student ID in token." });

            var result = await _enrollmentService.DropCourseAsync(studentId.Value, courseId);
            if (!result.Success)
                return Conflict(new { message = result.Message });

            return Ok(new { message = result.Message });
        }

        [Authorize(Roles = "Student")]
        [HttpGet("search")]
        public async Task<IActionResult> SearchCourses([FromQuery] string query) {
            var studentId = _userContextService.GetCurrentStudentId();
            if (studentId == null)
                return Unauthorized(new { message = "Invalid student ID in token." });

            var result = await _enrollmentService.SearchCoursesAsync(studentId.Value, query);
            if (!result.Success)
                return StatusCode(500, new { message = result.Message });

            return Ok(new { message = result.Message, data = result.Data });
        }
    }

        //private int? GetCurrentStudentId() {
        //    var studentIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //    if (int.TryParse(studentIdClaim, out var studentId))
        //        return studentId;

        //    return null;
        //}

    }

