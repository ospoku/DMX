
using DMX.Constants;
using DMX.Data;
using DMX.DataProtection;
using DMX.Helpers;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Web;
namespace DMX.ViewComponents
{
    public class ManageRolePermissions(RoleManager<AppRole>roleManager, XContext xContext) : ViewComponent
    {
        public readonly RoleManager<AppRole> rol = roleManager;
        public readonly XContext ctx=xContext;
        public async Task<IViewComponentResult> InvokeAsync(string Id)
        {
            var decodedId = HttpUtility.UrlDecode(Id)?.Replace(" ", "+"); // sanitize
            var decryptedId = Encryption.Decrypt(decodedId);
            var model = new RolePermissionVM();
            var allPermissions = new List<RoleClaimsVM>();
            
            allPermissions.GetPermissions(typeof(Permissions),decryptedId);
            var role = await rol.FindByIdAsync(decryptedId);
            model.RoleId = decryptedId;
            model.AvailableClaims=new SelectList(ctx.Permissions.ToList(),nameof(Permission.PermissionId),nameof(Permission.Code)).ToList();
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
