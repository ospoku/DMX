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
            var roles = _roleManager.Roles.ToList();
            var viewModelList = new List<ViewRolePermissionsVM>();

            foreach (var role in roles)
            {
                var appRole = await _roleManager.FindByIdAsync(role.Id);
                var claims = await _roleManager.GetClaimsAsync(appRole);
                var roleClaimCodes = claims.Select(c => c.Value).ToList();

                // ✅ Fetch all permissions from DB
                var allPermissions = await _context.Permissions
                    .Select(p => new PermissionVM
                    {
                        PermissionId = p.PermissionId,
                        PublicId = p.PublicId,
                        ModuleId = p.ModuleId,
                        Module = p.Module,
                        ActionId = p.ActionId,
                        Action = p.Action,
                        Code = p.Code,
                        Selected = roleClaimCodes.Contains(p.Code)
                    })
                    .ToListAsync();

                // ✅ Group by Module and Action for better display
                var groupedPermissions = allPermissions
                    .GroupBy(p => new { p.Module.Name })
                    .Select(g => new ViewRolePermissionsVM
                    {
                        ModuleName = g.Key.Name,
                        ActionName = g.Key.Name,
                        //Code = string.Join(",", g.Select(x => x.Code)),
                        //Description = $"Permissions under {g.Key.Name} module - {g.Key.Name} action",
                        //RoleClaims = g.ToList()
                    })
                    .ToList();

                viewModelList.AddRange(groupedPermissions);
            }

            return View(viewModelList);
        }
    }
}

