using System.ComponentModel.DataAnnotations;

namespace MyCoursesApp.Models.Dtos {
    public class CourseDto {
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public int CreditHours { get; set; }

        public string Description { get; set; }
    }
}
