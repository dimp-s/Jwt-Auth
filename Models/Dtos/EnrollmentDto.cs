namespace MyCoursesApp.Models.Dtos {
    public class EnrollmentDto {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int CreditHours { get; set; }
        public string Description { get; set; }
        public bool IsEnrolled { get; set; }
    }
}
