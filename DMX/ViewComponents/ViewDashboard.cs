using Microsoft.AspNetCore.Mvc;
using DMX.Data;
using DMX.ViewModels;

namespace DMX.ViewComponents
{
    public class ViewDashboard(XContext PrintContext) : ViewComponent
    {
        public readonly XContext prx = PrintContext;

        public IViewComponentResult Invoke()

        {
            ViewDashboardVM viewDashboardVM = new()
            {
                TotalDocuments = prx.Documents.Where(a => a.IsDeleted == false).Count().ToString(),
                //TotalFemales=prx.Documents.Where(a=>a.IsDeleted==false&a.Gender.GenderName=="Female").Count().ToString(),
                //TotalMales = prx.Documents.Where(a => a.IsDeleted == false & a.Gender.GenderName == "Male").Count().ToString(),
           
            };
        return View(viewDashboardVM);
           
        }
    }
}
