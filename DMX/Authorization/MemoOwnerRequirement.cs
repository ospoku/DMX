using Microsoft.AspNetCore.Authorization;

namespace DMXApplication.Authorization
{
    public class MemoOwnerRequirement : IAuthorizationRequirement
    {
        public MemoOwnerRequirement()
        {
            // Add any properties needed for the requirement if necessary
        }
    }

}