using DMX.Constants;
using DMX.Data;
using DMX.Helpers;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMX.ViewComponents
{
    public class ViewRolePermissions : ViewComponent
    {
        private readonly XContext _context;
        private readonly RoleManager<AppRole> _roleManager;

        public ViewRolePermissions(XContext context, RoleManager<AppRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var viewModelList = new List<ViewRolePermissionsVM>();

            // Load all permissions once
            var allPermissions = await _context.Permissions
                .Select(p => new PermissionVM
                {
                    PermissionId = p.PermissionId,
                    PublicId = p.PublicId,
                    Module = p.Module,
                    Action = p.Action,
                    Code = p.Code,
                })
                .ToListAsync();

            foreach (var role in roles)
            {
                var claims = await _roleManager.GetClaimsAsync(role);
                var roleClaimCodes = claims.Select(c => c.Value).ToList();

                var grouped = allPermissions
                    .GroupBy(p => new { p.Module, p.Action })
                    .Select(g => new ViewRolePermissionsVM
                    {
                        RoleId = role.Id,
                        RoleName = role.Name,

                        Module = g.Key.Module,
                        Action = g.Key.Action,
                        PermissionCodes = g.Select(x => x.Code).ToList(),

                        // ✔ Mark Selected = true if ANY code in the group is in the role’s claims
                        Selected = g.Any(p => roleClaimCodes.Contains(p.Code)),

                        // ✔ Pass role claims for this group (optional)
                        RoleClaims = roleClaimCodes
                    })
                    .ToList();

                viewModelList.AddRange(grouped);
            }

            return View(viewModelList);
        }
    }
}
