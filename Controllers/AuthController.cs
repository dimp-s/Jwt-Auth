
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
            var user = await _authService.RegisterAsync(request);
            if (user == null) return BadRequest("User already Exists!");
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginDto request) {
            var token = await _authService.LoginAsync(request);
            if (token == null) return BadRequest("Invalid Username or Password");
            return Ok( new {token});
        }

    }
}