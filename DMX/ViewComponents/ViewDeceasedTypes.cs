using Microsoft.AspNetCore.Mvc;
using DMX.Data;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using DMX.Models;

namespace DMX.ViewComponents
{
    public class ViewDeceasedTypes(XContext dContext, UserManager<AppUser>userManager) : ViewComponent
    {
        public readonly XContext dcx = dContext;

        private readonly UserManager<AppUser> usm = userManager;

        public IViewComponentResult Invoke()
        {
            var user = usm.GetUserAsync(HttpContext.User).Result.UserName;
            var dTypes = dcx.DeceasedTypes.Where(a => a.IsDeleted == false).Select(a => new ViewDTypesVM
            {
                TypeId = a.Id,
                Code = a.Code,
                Name= a.Name,
            Description = a.Description,
            
            }).ToList();
            return View(dTypes);
        }
    }
}
