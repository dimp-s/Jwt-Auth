using System.Security.Claims;

namespace MyCoursesApp.Services.Interfaces {
    public interface IUserContextService {
        int? GetCurrentStudentId();
        ClaimsPrincipal User { get; }

    }
}
