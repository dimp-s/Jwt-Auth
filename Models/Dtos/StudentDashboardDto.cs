namespace MyCoursesApp.Models.Dtos {
    public class StudentDashboardDto {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileImage { get; set; }
        public List<CourseInfo> EnrolledCourses { get; set; }

        public class CourseInfo {
            public int CourseId { get; set; }
            public string Name { get; set; }
            public int CreditHours { get; set; }
            //public DateTime EnrolledOn { get; set; }
            public DateTime EnrolledOn { get; set; }
        }
    }
}
