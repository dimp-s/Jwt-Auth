using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyCoursesApp.Migrations
{
    /// <inheritdoc />
    public partial class AdminSeeder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "PasswordHash", "ProfileImage", "RoleId" },
                values: new object[] { 1, "admin@admin.com", "Super", "Admin", "AQAAAAIAAYagAAAAEMRQf/LxHtOosxwR+/g1ZHhkJyM8X9YP+vVqvojZfzPsgYtGPf7dMpvlG36h0otmXw==", "", 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
