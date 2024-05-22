using DMX.DataProtection;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMX.ViewComponents
{
    public class UserProfile(UserManager<AppUser>userManager):ViewComponent
    {
        public readonly UserManager<AppUser> usm = userManager;
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

            };




            return View();
        }
    }
}
