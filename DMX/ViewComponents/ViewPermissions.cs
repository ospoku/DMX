using DMX.Constants;
using DMX.Data;
using DMX.DataProtection;
using DMX.Helpers;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DMX.ViewComponents
{
    public class ViewPermissions(XContext xContext, UserManager<AppUser> userManager,RoleManager<AppRole> roleManager) : ViewComponent
    {
        public readonly XContext dcx = xContext;
        public readonly UserManager<AppUser> usm = userManager;
        public readonly RoleManager<AppRole> rol = roleManager;
        public IViewComponentResult Invoke()
        {
        
            var permissions = new List<Permissions>();

            foreach (var perm in permissions)
            {
                

         var vp   = new    ViewPermissionsVM
                {
                 AssignedPermissions=perm.
                });
            }

            return View(result);
        }
    }
}

