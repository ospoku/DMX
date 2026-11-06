using DMXApplication.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DMXApplication.Services
{
   
        public class CurrentUserService(
    IHttpContextAccessor httpContextAccessor) : ICurrentUserService
        {
            public string UserId
        {
            get
            {
                ClaimsPrincipal user = httpContextAccessor.HttpContext.User;
                return user?.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            }
        }
        }
    }

