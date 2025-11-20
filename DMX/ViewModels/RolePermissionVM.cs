using System.Collections.Generic;

namespace DMX.ViewModels
{
    public class RolePermissionVM
    {
        
        public Guid PublicId { get; set; }
        public string RoleId { get; set; }
        public IList<RoleClaimsVM> RoleClaims { get; set; }
    }

}
