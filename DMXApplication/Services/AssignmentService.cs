using DMXDomain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DMXApplication.Services
{
    public class AssignmentService(UserManager<AppUser> userManager, XContext context, INotyfService notyfService)
    {
        private readonly UserManager<AppUser> usm = userManager;
        private readonly INotyfService notyf = notyfService;

        private readonly DbContext dcx = context;

        public async Task<bool> AssignUsers<T>(T model, ClaimsPrincipal userClaims) where T : class
        {
            var userId = usm.GetUserId(userClaims);

            model.GetType().GetProperty("CreatedBy")?.SetValue(model, userId, null);
            model.GetType().GetProperty("CreatedDate")?.SetValue(model, DateTime.UtcNow);
            try
            {
                dcx.Set<T>().Add(model);
                if (await dcx.SaveChangesAsync(userId) > 0)
                { 
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                notyf.Error("An error occurred: " + ex.Message, 5);
                return false;
            }
        }

    }
}

