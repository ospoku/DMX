using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMX.ViewComponents
{
    public class AddWorkshop() : ViewComponent
    {
        

        public IViewComponentResult Invoke()
        {
            AddWorkshopVM addWorkshopVM = new()
            {
            };
            
        

            return View(addWorkshopVM);
        }
    }
}
