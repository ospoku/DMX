using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using DMX.ViewModels;
using DMX.Models;

namespace DMX.ViewComponents
{
    public class AddTraining(UserManager<AppUser> userManager) : ViewComponent
    {
        public readonly UserManager<AppUser> usm = userManager;

        public IViewComponentResult Invoke()
        {
            AddTrainingVM addTrainingVM = new()
            {

            


            };

            return View(addTrainingVM);
        }

    }
}
