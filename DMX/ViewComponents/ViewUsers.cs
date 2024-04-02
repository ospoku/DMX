using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DMX.Data;
using DMX.Models;
using DMX.ViewModels;

namespace DMX.ViewComponents
{
    public class ViewUsers(XContext printContext, UserManager<AppUser> userManager) : ViewComponent
    {
        public readonly XContext dcx = printContext;
        public readonly UserManager<AppUser> usm;

        public IViewComponentResult Invoke()
        { 
            var userList = dcx.Users.Where(u => u.IsDeleted == false).Select(u => new ViewUsersVM
            {
                UserId = u.Id,
                Fullname = u.Fullname,
              

                Username = u.UserName,
                Email=u.Email,
                Telephone=u.PhoneNumber,
              
               Role= string.Join(",", from p in dcx.UserRoles
                                       join role in dcx.Roles on p.RoleId equals role.Id
                                       where p.UserId == u.Id
                                       select role.Name.ToString())
                

            }).ToList();

            return View(userList);
        }
    }
}
