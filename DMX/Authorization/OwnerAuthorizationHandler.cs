using DMX.Data;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace DMX.Authorization
{
    public class OwnerAuthorizationHandler : AuthorizationHandler<OwnerRequirement, TableAudit>
    {


        protected override  Task HandleRequirementAsync(AuthorizationHandlerContext context, OwnerRequirement requirement, TableAudit resource)
        {
            if (context.User.HasClaim(ClaimTypes.NameIdentifier, resource.CreatedBy))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}