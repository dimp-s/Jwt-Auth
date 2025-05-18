using MyCourses.Models;

namespace MyCoursesApp.Models {
    public class Enrollment {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }

        public DateTime EnrolledOn { get; set; }

        public virtual User User { get; set; }
        public virtual Course Course { get; set; }
    }
}
