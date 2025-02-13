using DMX.Data;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Xml.Linq;

namespace DMX.ViewComponents
{
    public class AddServiceRequest:ViewComponent
    {
        public readonly UserManager<AppUser> usm;
        public readonly XContext xct;
        public AddServiceRequest(UserManager<AppUser> userManager,XContext context)
        {
            usm = userManager;
            xct = context;
        }
        public IViewComponentResult Invoke()
        {
            AddServiceRequestVM addServiceRequest = new AddServiceRequestVM
            {
                UsersList = new SelectList(usm.Users.ToList(), "Id", "UserName"),
                Status=new SelectList(xct.Statuses.ToList(), "Id","Name"),
                Categories = xct.Categories.Select(c=>new CheckBoxItem {Id=c.Id,Name=c.Name
                }).ToList()
            };
            return View(addServiceRequest);
        }
    }
}
