﻿using Microsoft.EntityFrameworkCore;
using MyCourses.Models;
using MyCoursesApp.Models;

namespace MyCoursesApp.Data {
    public class AppDbContext: DbContext {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        public DbSet<Role> Roles {  get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.User)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.UserId);
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId);
            // Seeding roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "Student" }
            );
            //seed an admin user
            var adminUser = new User {
                Id = 1,
                FirstName = "Super",
                LastName = "Admin",
                Email = "admin@admin.com",
                PasswordHash = "AQAAAAIAAYagAAAAEMRQf/LxHtOosxwR+/g1ZHhkJyM8X9YP+vVqvojZfzPsgYtGPf7dMpvlG36h0otmXw==",
                ProfileImage = "",
                RoleId = 1 // Admin role
            };

            modelBuilder.Entity<User>().HasData(adminUser);

            //seeding courses
            modelBuilder.Entity<Course>().HasData(
             new Course { Id = 1, Name = "Intro to Java", CreditHours = 8, Description = "Beginner friendly course to learn JAVA programming. Get Started now with improved lessons." },
             new Course { Id = 2, Name = "Intro to C#", CreditHours = 4, Description = "Beginner friendly course to learn c# programming. Get Started now with improved lessons." },
             new Course { Id = 3, Name = "AWS Beginner Pack", CreditHours = 12, Description = "Beginner friendly course to start with AWS. Get Started now with improved lessons." }
             );
        }
    }
}
