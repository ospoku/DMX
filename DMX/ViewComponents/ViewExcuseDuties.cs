using Microsoft.AspNetCore.Mvc;
using DMX.Data;
using DMX.ViewModels;

namespace DMX.ViewComponents
{
    public class ViewExcuseDuties : ViewComponent
    {
        public readonly XContext dcx;
        public ViewExcuseDuties(XContext dContext)
        {
            dcx = dContext;
        }
        public IViewComponentResult Invoke()
        {
            var lList = dcx.ExcuseDuties.Where(t => t.IsDeleted == false).Select(t => new ViewExcuseDutiesVM
            {

                Name=t.Name,
                    
      Date=t.Date,
      DateofDischarge=t.DateofDischarge,
      ExcuseDays=t.ExcuseDays,
      OperationDiagnosis=t.OperationDiagnosis,





    }).ToList();
            return View(lList);
        }
    }
}
