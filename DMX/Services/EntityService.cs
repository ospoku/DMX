using AspNetCoreHero.ToastNotification.Abstractions;
using DMX.Data;
using DMX.Helpers;
using DMX.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DMX.Services
{
    public class EntityService
    {
        private readonly UserManager<AppUser> usm;
        private readonly INotyfService notyf;
       
        private readonly XContext dcx;

        public EntityService(UserManager<AppUser> userManager, XContext context, INotyfService notyfService)
        {
            usm = userManager;
            notyf = notyfService;
            dcx = context;
        }

        public async Task<IActionResult> AddEntityAsync<T>(T entity, ClaimsPrincipal userClaim) where T : class
        {
            var user = await usm.GetUserAsync(userClaim);
            if (user == null)
            {
                notyf.Error("User is not authenticated.", 5);
                return new RedirectToActionResult("Login", "Account", null);  // Adjust as necessary
            }

            entity.GetType().GetProperty("CreatedBy")?.SetValue(entity, user.UserName);
            entity.GetType().GetProperty("CreatedDate")?.SetValue(entity, DateTime.UtcNow);

            try
            {
                dcx.Set<T>().Add(entity);
                if (await dcx.SaveChangesAsync(user.UserName) > 0)
                {
                    notyf.Success("Record successfully saved!", 5);
                    return new OkResult();  // Return success response
                }
                else
                {
                    notyf.Error("Error, record could not be saved.", 5);
                    return new BadRequestResult();  // Return failure response
                }
            }
            catch (Exception ex)
            {
                notyf.Error("An error occurred: " + ex.Message, 5);
                return new StatusCodeResult(500);  // Internal server error
            }
        }
    }
}
