﻿namespace MyCoursesApp.Models.Dtos {
    public class LoginDto {
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }
}
