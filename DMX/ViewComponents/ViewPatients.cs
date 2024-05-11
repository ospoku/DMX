using Microsoft.AspNetCore.Mvc;
using DMX.Data;
using DMX.ViewModels;
namespace DMX.ViewComponents
{
    public class ViewPatients(XContext dContext) : ViewComponent
    {
        public readonly XContext dcx = dContext;

        public IViewComponentResult Invoke()
        {
            var pList = dcx.Patients.Where(a => a.IsDeleted == false).Select(a => new ViewPatientsVM
            {







                CreatedDate = a.CreatedDate,
            }).OrderByDescending(t => t.CreatedDate).ToList();
            return View(pList);
        }
    }
}
