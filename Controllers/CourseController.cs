using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyCoursesApp.Models.Dtos;
using MyCoursesApp.Services.Interfaces;

namespace MyCoursesApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService) {
            _courseService = courseService;
        }

        // GET: Courses
        [HttpGet]
        public async Task<IActionResult> Index() {
            var result = await _courseService.GetAllAsync();
            return result.Success
                ? Ok(result)
                : NotFound(result);
        }

        // GET: Courses/Details/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Details(int? id) {
            var result = await _courseService.GetDetailsAsync(id);
            return result.Success
                ? Ok(result)
                : NotFound(result);
        }


        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CourseDto course) {
            //if (ModelState.IsValid) {
            //    await _courseService.CreateAsync(course);
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(course);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _courseService.CreateAsync(course);
            return result.Success
                ? Ok(result)
                : BadRequest(result);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Edit(int id, CourseDto updatedCourse) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existsResult = await _courseService.GetDetailsAsync(id);
            if (!existsResult.Success)
                return NotFound(existsResult);

            var result = await _courseService.UpdateAsync(id, updatedCourse);
            return result.Success
                ? Ok(result)
                : BadRequest(result);
        }

        // POST: Courses/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var result = await _courseService.DeleteAsync(id);
            return result.Success
                ? Ok(result)
                : NotFound(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult GetAdminData() {
            return Ok("This is protected data for Admins.");
        }
        private bool CourseExists(int id) {
            return _courseService.CourseExists(id);
        }
    }
}

