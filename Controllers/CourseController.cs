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

            try {
                var courses = await _courseService.GetAllAsync();
                if (courses == null || !courses.Any())
                    return Ok(new { message = "No Courses found!" });
                return Ok(new { message = "All Courses Returned", data = courses });
            } catch (Exception ex) {
                return StatusCode(500, new { message= "An Error occured!", ex.Message});
            }
            
        }

        // GET: Courses/Details/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Details(int? id) {
            try {
                var course = await _courseService.GetDetailsAsync(id);
                if(course == null) {
                    return NotFound(new { message = "Course Not Found!" });
                }
                return Ok(new { message = "Course retrieved!", data = course });

            }
            catch (Exception ex) {
                return StatusCode(500, new { message = $"An error occurred while retrieving!", error = ex.Message });
            }
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
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            try {
                await _courseService.CreateAsync(course);
                return Ok(new { message = "Course Created", course });
            }
            catch (Exception ex) { 
                return StatusCode(500, new { message= "An error occured!",error = ex.Message });
            }
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Edit(int id, CourseDto updatedCourse) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            try {
                var course = await _courseService.GetDetailsAsync(id);
                if (course == null) {
                    return NotFound(new { message = "Course Not Found!" });
                }
                await _courseService.UpdateAsync(id, updatedCourse);
                return Ok(new { message = "Course Updated!" });

            }
            catch (Exception ex) {
                return StatusCode(500, new { message = "An error occured when Updating the course", error = ex.Message });
            }
        }

        // POST: Courses/Delete/5
        [HttpDelete("{id:int}")]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            try {
                await _courseService.DeleteAsync(id);
                return Ok(new { message = "Course deleted!" });
            }
            catch (Exception ex) {
                return StatusCode(500, new { message = "Error occured in deleting course", error = ex.Message });
            }
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

