using Microsoft.AspNetCore.Mvc;

namespace DMX.ViewComponents
{
    public class AddTravelType:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
