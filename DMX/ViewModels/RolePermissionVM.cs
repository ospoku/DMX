using System.Collections.Generic;

namespace DMX.ViewModels
{
    public class RolePermissionVM
    {
        public string RoleId { get; set; }
        public IList<RoleClaimsVM> RoleClaims { get; set; }
    }

}
