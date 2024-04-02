using Microsoft.AspNetCore.Mvc;
using DMX.Data;
using DMX.ViewModels;

namespace DMX.ViewComponents
{
    public class ViewSickReports(XContext dContext) : ViewComponent
    {
        public readonly XContext dcx = dContext;

        public IViewComponentResult Invoke()
        {
            var lList = dcx.SickReports.Where(t => t.IsDeleted == false).Select(t => new ViewSickReportsVM
            {

             SickReportId=t.SickReportId





    }).ToList();
            return View(lList);
        }
    }
}
