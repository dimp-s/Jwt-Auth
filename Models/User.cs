using MyCoursesApp.Models;

namespace MyCourses.Models {
    public class User {

        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; }
        public string ProfileImage { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
