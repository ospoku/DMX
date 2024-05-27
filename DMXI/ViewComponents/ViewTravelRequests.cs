using Microsoft.AspNetCore.Mvc;
using DMX.Data;
using DMX.ViewModels;

namespace DMX.ViewComponents
{
    public class ViewTravelRequests(XContext dContext) : ViewComponent
    {
        public readonly XContext dcx = dContext;

        public IViewComponentResult Invoke()
        {
            var lList = dcx.TravelRequests.Where(t => t.IsDeleted == false).Select(t => new ViewTravelRequestsVM
            {








                CreatedDate = t.CreatedDate,
            }).OrderByDescending(t => t.CreatedDate).ToList();
            return View(lList);
        }
    }
}
