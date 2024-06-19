using System.Security.Claims;

namespace PatientDashboardService.Services.Service
{
    
        public class AuthService
        {
            private readonly IHttpContextAccessor _httpContextAccessor;

            public AuthService(IHttpContextAccessor httpContextAccessor)
            {
                _httpContextAccessor = httpContextAccessor;
            }

            public string GetCurrentUserId()
            {
                // Retrieve the UserId from the current user's claims
                return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }
        
    }
}
