using Microsoft.EntityFrameworkCore;
using MyCourses.Models;

namespace MyCoursesApp.Data {
    public class AppDbContext: DbContext {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
    }
}
