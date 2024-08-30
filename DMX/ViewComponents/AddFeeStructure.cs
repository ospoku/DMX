using Microsoft.AspNetCore.Mvc;

namespace DMX.ViewComponents
{
    public class AddFeeStructure:ViewComponent
    {
        public IViewComponentResult InvokeAsync()
        {
            return View();
        }
    }
}
