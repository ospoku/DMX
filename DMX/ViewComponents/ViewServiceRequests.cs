using Microsoft.AspNetCore.Mvc;
using DMX.Data;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using DMX.Models;
namespace DMX.ViewComponents
{
    public class ViewServiceRequests(XContext dContext, UserManager<AppUser>userManager) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        public readonly  UserManager<AppUser> usm=userManager;

        public IViewComponentResult Invoke()
        {
            var user = usm.GetUserAsync(HttpContext.User).Result?.UserName;
            var servList = dcx.ServiceAssignments.Where(a => a.AppUser.UserName == user || a.ServiceRequest.CreatedBy == user).Select(a => new 
             ViewServiceRequestsVM
            {
                
            Faults = a.ServiceRequest.Faults,  
           
            RequestNumber = a.ServiceRequest.RequestNumber,
          
                CreatedDate = a.ServiceRequest.CreatedDate,
            }).OrderByDescending(t => t.CreatedDate).ToList();
            return View(servList);
        }
    }
}
