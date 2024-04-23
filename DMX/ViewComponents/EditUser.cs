using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DMX.Data;
using DMX.Models;
using DMX.ViewModels;
using DMX.DataProtection;

namespace DMX.ViewComponents
{
    public class EditUser(UserManager<AppUser> userManager, RoleManager<AppRole> rolManager) : ViewComponent
    {
        public readonly UserManager<AppUser> usm = userManager;
        public readonly RoleManager<AppRole> rol = rolManager;
        

        public IViewComponentResult Invoke(string Id)
        {

            AppUser userToEdit = (from u in usm.Users where u.Id == @Encryption.Decrypt(Id) select u).FirstOrDefault();

            EditUserVM editUserVM = new()
            {
              
                Email = userToEdit.Email,
                Firstname = userToEdit.Firstname,
                Username = userToEdit.UserName,
                Surname = userToEdit.Surname,
                Telephone = userToEdit.PhoneNumber,
               ApplicationRoleId = rol.Roles.Single(r => r.Name == usm.GetRolesAsync(userToEdit).Result.Single()).Id,
              
                ApplicationRoles = [.. rol.Roles.Select(r => new SelectListItem { Text = r.Name, Value = r.Id })],
            };

            return View(editUserVM);
        }
    }
}
