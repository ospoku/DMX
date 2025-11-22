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

                // Only permission claims
                var roleClaimCodes = claims
                    .Where(c => c.Type == "Permissions")
                    .Select(c => c.Value)
                    .ToList();

                // Group permissions by Module (one row per Module)
                var grouped = allPermissions
                    .GroupBy(p => p.Module)
                    .Select(g => new ViewRolePermissionsVM
                    {
                        RoleId = role.Id,
                        RoleName = role.Name,

                        Module = g.Key,

                        // All permission codes under the module
                        PermissionCodes = g.Select(x => x.Code).ToList(),

                        // Only those the role has
                        SelectedPermissions = g
                            .Where(x => roleClaimCodes.Contains(x.Code))
                            .Select(x => x.Code)
                            .ToList()
                    }).Distinct()
                    .ToList();

                viewModelList.AddRange(grouped);
            }

            return View(viewModelList);
        }

    }
}
