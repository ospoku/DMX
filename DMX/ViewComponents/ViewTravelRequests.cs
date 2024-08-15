using Microsoft.AspNetCore.Mvc;
using DMX.Data;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using DMX.Models;

namespace DMX.ViewComponents
{
    public class ViewTravelRequests(XContext dContext,UserManager<AppUser>userManager) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser>usm= userManager;
        public IViewComponentResult Invoke()
        {
            var user = usm.GetUserAsync(HttpContext.User).Result?.UserName;
            var travelList = dcx.TravelRequestAssignments.Where(a => a.AppUser.UserName == user || a.TravelRequest.CreatedBy == user).Select(a => 
             new ViewTravelRequestsVM
            {


                CreatedDate = a.CreatedDate,
            }).OrderByDescending(t => t.CreatedDate).ToList();
            return View(travelList);
        }
    }
}
