using DMX.Data;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DMX.ViewComponents
{
    public class AddExternalTraining : ViewComponent
    {
        public readonly XContext dcx;
        public readonly UserManager<AppUser> usm;


        public AddExternalTraining(XContext dContext, UserManager<AppUser> userManager)
        {
            dcx = dContext;
            usm = userManager;

        }

        public IViewComponentResult Invoke()
        {

            AddExternalTraningVM trainingVM = new ()
            {

            };






            return View(trainingVM);
        }
    }
}

