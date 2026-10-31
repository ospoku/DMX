using DMXApplication.Interfaces;
using DMXDomain.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DMXApplication.Services
{
    public class AssignmentService(
        UserManager<AppUser> userManager,
       
        IAssignmentRepository assignmentRepository)
    {
        private readonly UserManager<AppUser> usm = userManager;
  
        private readonly IAssignmentRepository assRep = assignmentRepository;

        public async Task<bool> AssignUsers<T>(
            T model,
            ClaimsPrincipal userClaims) where T : class
        {
            var userId = usm.GetUserId(userClaims);

            if (string.IsNullOrEmpty(userId))
            {
              
                return false;
            }

            try
            {
                model.GetType().GetProperty("CreatedBy")
                    ?.SetValue(model, userId);

                model.GetType().GetProperty("CreatedDate")
                    ?.SetValue(model, DateTime.UtcNow);

                assRep.Add(model);

                return await assRep.SaveChangesAsync(userId) > 0;
            }
            catch (Exception)
            {
               
                return false;
            }
        }
    }
}