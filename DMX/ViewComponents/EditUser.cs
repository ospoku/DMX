using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DMX.Data;
using DMX.Models;
using DMX.ViewModels;

namespace DMX.ViewComponents
{
    public class EditUser(UserManager<AppUser> userManager, RoleManager<AppRole> rolManager, XContext PrintContext) : ViewComponent
    {
        public readonly UserManager<AppUser> USM = userManager;
        public readonly RoleManager<AppRole> ROL = rolManager;
        public readonly XContext prx = PrintContext;

        public IViewComponentResult Invoke(string Id)
        {

            AppUser userToEdit = (from u in USM.Users where u.Id == Id select u).FirstOrDefault();

            EditUserVM editUserVM = new()
            {
              
                Email = userToEdit.Email,
                Firstname = userToEdit.Firstname,
                Username = userToEdit.UserName,
                Surname = userToEdit.Surname,
                Telephone = userToEdit.PhoneNumber,
               ApplicationRoleId = ROL.Roles.Single(r => r.Name == USM.GetRolesAsync(userToEdit).Result.Single()).Id,
              
                ApplicationRoles = ROL.Roles.Select(r => new SelectListItem { Text = r.Name, Value = r.Id }).ToList(),
            };

            return View(editUserVM);
        }
    }
}
