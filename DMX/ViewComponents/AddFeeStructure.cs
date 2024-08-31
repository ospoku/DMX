using DMX.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DMX.ViewComponents
{
    public class AddFeeStructure:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            
            return View(new AddFeeStructureVM());
        }
    }
}
