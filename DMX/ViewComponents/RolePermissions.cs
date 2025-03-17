
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DMX.ViewModels;
using DMX.Models;
using DMX.DataProtection;
using DMX.Helpers;
using DMX.Constants;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
namespace DMX.ViewComponents
{
    public class RolePermissions(RoleManager<AppRole>roleManager) : ViewComponent
    {
        public readonly RoleManager<AppRole> rol = roleManager;
        public async Task<IViewComponentResult> InvokeAsync(string Id)
        {
            var model = new RolePermissionVM();
            var allPermissions = new List<RoleClaimsVM>();

            allPermissions.GetPermissions(typeof(Permissions.Modules), @Encryption.Decrypt(Id));
            var role = await rol.FindByIdAsync(@Encryption.Decrypt(Id));
            model.RoleId = @Encryption.Decrypt(Id);
            var claims = await rol.GetClaimsAsync(role);
            var allClaimValues = allPermissions.Select(a => a.Value).ToList();
            var roleClaimValues = claims.Select(a => a.Value).ToList();
            var authorizedClaims = allClaimValues.Intersect(roleClaimValues).ToList();
            foreach (var permission in allPermissions)
            {
                if (authorizedClaims.Any(a => a == permission.Value))
                {
                    permission.Selected = true;
                }
            }
            model.RoleClaims = allPermissions;
            return View(model);
        }
        }
    }
