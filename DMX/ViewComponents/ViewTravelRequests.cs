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
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = (await usm.GetUserAsync(HttpContext.User)).Id;
            var travelList = dcx.TravelRequestAssignments.Where(a => a.AppUser.Id == user || a.TravelRequest.CreatedBy == user & a.TravelRequest.IsDeleted == false).Select(a => 
             new ViewTravelRequestsVM
            {
                 ReferenceNumber=a.TravelRequest.ReferenceNumber,
                 TravelType=a.TravelRequest.TravelType.Name,
                 Name=usm.FindByIdAsync(a.TravelRequest.CreatedBy).Result.Fullname,
                 DepartureDate=a.TravelRequest.StartDate,
                 TravelRequestId=a.TravelRequestId,
                 PurposeofJourney=a.TravelRequest.Purpose,
                 StartDate=a.TravelRequest.EndDate,
                 TotalAllowance= a.TravelRequest.TotalAllowance(a.TravelRequest.StartDate, a.TravelRequest.EndDate),
                 DateofReturn =a.TravelRequest.DateofReturn,
                CreatedDate = a.CreatedDate,
            }).OrderByDescending(t => t.CreatedDate).ToList();
            return View(travelList);
        }
    }
}
