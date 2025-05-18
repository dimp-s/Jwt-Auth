using System.ComponentModel.DataAnnotations;

namespace MyCoursesApp.Models {
    public class Course {
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public int CreditHours { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
