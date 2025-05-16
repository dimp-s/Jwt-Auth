using System.ComponentModel.DataAnnotations;

namespace MyCoursesApp.Models.Dtos {
    public class RegisterDto {
 
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }
}
