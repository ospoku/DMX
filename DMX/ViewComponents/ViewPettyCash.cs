using Microsoft.AspNetCore.Mvc;
using DMX.Data;
using DMX.ViewModels;
namespace DMX.ViewComponents
{
    public class ViewPettyCash(XContext dContext) : ViewComponent
    {
        public readonly XContext dcx = dContext;

        public IViewComponentResult Invoke()
        {
            var pcList = dcx.PettyCashes.Where(a => a.IsDeleted == false).Select(a => new ViewPettyCashVM
            {
                PettyCashId = a.PettyCashId,
                Amount = a.Amount,
                Name= a.Name,   
                Date = a.Date,
                Description= a.Description, 
                Purpose=a.Purpose,
            }).ToList();
            return View(pcList);
        }
    }
}
