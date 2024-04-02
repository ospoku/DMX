using DMX.Data;
using DMX.DataProtection;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DMX.ViewComponents
{
    public class ManageUserRoles(XContext dContext, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser> usm = userManager;
        public readonly RoleManager<AppRole> rol = roleManager;

        public async Task<IViewComponentResult> InvokeAsync(string Id)
        {
            var viewModel = new List<UserRolesVM>();
            var user = await usm.FindByIdAsync(@Encryption.Decrypt(Id));

            foreach (var role in rol.Roles.ToList())
            {
                var userRolesViewModel = new UserRolesVM
                {
                    RoleName = role.Name
                };
                if (await usm.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }
                viewModel.Add(userRolesViewModel);
            }
            var model = new ManageUserRolesVM()
            {
                UserId = @Encryption.Decrypt(Id),

                UserRoles = viewModel
            };
            return View(model);
        }
    }
}
