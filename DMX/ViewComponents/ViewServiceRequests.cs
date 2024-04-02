using Microsoft.AspNetCore.Mvc;
using DMX.Data;
using DMX.ViewModels;
namespace DMX.ViewComponents
{
    public class ViewServiceRequests(XContext dContext) : ViewComponent
    {
        public readonly XContext dcx = dContext;

        public IViewComponentResult Invoke()
        {
            var sList = dcx.ServiceRequests.Where(s => s.IsDeleted == false).Select(s => new ViewServiceRequestsVM
            {
                ServiceRequestedBy = s.RequestedBy,







            }).ToList();
            return View(sList);
        }
    }
}
