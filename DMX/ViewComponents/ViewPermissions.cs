using DMX.Data;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DMX.ViewComponents
{
    public class ViewPermissions(XContext xContext, UserManager<AppUser> userManager):ViewComponent
    {
        public readonly XContext dcx = xContext;
        public readonly UserManager<AppUser> usm = userManager;
        public IViewComponentResult Invoke()
        {
            var userList = usm.Users.Where(u => u.IsDeleted == false).Select(u => new ViewUsersVM
            {
                UserId = u.Id,
                Fullname = u.Fullname,
                Username = u.UserName,
                Email = u.Email,
            }).ToList();

            return View(userList);
        }
    }
}
