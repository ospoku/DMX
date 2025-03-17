using Microsoft.AspNetCore.Mvc;

namespace DMX.ViewComponents
{
    public class AnnualLeave : ViewComponent
    {

        public AnnualLeave()
        {

        }

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
