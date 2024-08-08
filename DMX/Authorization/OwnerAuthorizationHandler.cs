using DMX.Data;
using DMX.Models;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace DMX.Authorization
{
    public class OwnerAuthorizationHandler : AuthorizationHandler<OwnerRequirement, Memo>
    {


        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OwnerRequirement requirement, Memo resource)
        {
            if (context.User.HasClaim(ClaimTypes.Name,resource.CreatedBy.ToString()))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}