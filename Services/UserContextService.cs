using System.Security.Claims;
using MyCoursesApp.Services.Interfaces;

namespace MyCoursesApp.Services {
    public class UserContextService : IUserContextService {

        private readonly IHttpContextAccessor _contextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor) {
            _contextAccessor = httpContextAccessor;
        }
        public ClaimsPrincipal User => _contextAccessor.HttpContext?.User;

        public int? GetCurrentStudentId() {
            var studentIdClaim =  User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(studentIdClaim, out var studentId))
                return studentId;

            return null;
        }
    }
}
