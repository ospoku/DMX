using DMX.Authorization;
using DMXDomain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DMXApplication.Authorization
{
    public class MemoOwnerHandler(UserManager<AppUser> userManager) : AuthorizationHandler<MemoOwnerRequirement, Memo>
    {

        private readonly UserManager<AppUser> usm = userManager;

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            MemoOwnerRequirement requirement,
            Memo resource)
        {
            // Get logged-in user object
            var user = await usm.GetUserAsync(context.User);
          
            if (user == null)
            {
                return;
            }

            // Compare logged-in user with memo owner
            if (context.User.HasClaim(x=>x.Type==ClaimTypes.NameIdentifier && x.Value== resource.CreatedBy))
            {
                context.Succeed(requirement);
            }
        }
    }
}
