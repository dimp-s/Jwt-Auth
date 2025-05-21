
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyCoursesApp.Services.Interfaces;

namespace MyCoursesApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class StudentDashboardController : ControllerBase {

        private readonly IStudentDashboardService _studentDashboardService;
        private readonly IUserContextService _userContextService;
        public StudentDashboardController(IStudentDashboardService studentDashboardService, IUserContextService userContextService) {
            _studentDashboardService = studentDashboardService;
            _userContextService = userContextService;
        }

        [Authorize]
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboardData() {
            try {
                var userId = _userContextService.GetCurrentStudentId();
                if (userId == null) return Unauthorized(new { message = "Invalid Student ID in token." });

                var dashboardData = await _studentDashboardService.GetDashboardDataAsync(userId);
                if (dashboardData == null) return NotFound(new { message = "Not Found!" });

                return Ok(new { message = "Dashboard Data recieved", data = dashboardData });
            }
            catch (Exception ex) {
                return StatusCode(500, new { message = "Error occured in deleting course", error = ex.Message });
            }

        }

    }
}
