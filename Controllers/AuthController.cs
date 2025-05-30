using Microsoft.AspNetCore.Mvc;
using MyCourses.Models;
using MyCoursesApp.Models.Dtos;
using MyCoursesApp.Services.Interfaces;

namespace MyCoursesApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService) {
            _authService = authService;
        }




        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterDto request) {
            var result = await _authService.RegisterAsync(request);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginDto request) {
            var result = await _authService.LoginAsync(request);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

    }
}