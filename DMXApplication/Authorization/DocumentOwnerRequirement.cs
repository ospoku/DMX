using Microsoft.AspNetCore.Authorization;

namespace DMXApplication.Authorization
{
    public class DocumentOwnerRequirement : IAuthorizationRequirement
    {
        public DocumentOwnerRequirement()
        {
            // Add any properties needed for the requirement if necessary
        }
    }

}