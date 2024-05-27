using DMX.Data;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMX.ViewComponents
{
    public class AddTravelRequest(UserManager<AppUser> userManager, XContext xContext) : ViewComponent
    {
        public readonly UserManager<AppUser> usm = userManager;
        public readonly XContext dcx = xContext;
        public IViewComponentResult Invoke()
        {
            AddTravelRequestVM addTravelRequestVM = new AddTravelRequestVM
            {
                TravelTypes = new SelectList(dcx.TravelTypes.ToList(), "TravelTypeId", "Name"),
                UsersList = new SelectList(usm.Users.ToList(), "Id", "UserName")
            };

            return View(addTravelRequestVM);
        }
    }
}
