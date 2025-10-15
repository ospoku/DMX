using Microsoft.AspNetCore.Mvc;

namespace DMX.ViewComponents
{
    public class AccountSetup:ViewComponent
    {

        public IViewComponentResult Invoke()
        {
            return View();  
        }
    }
}
