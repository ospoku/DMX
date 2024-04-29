using DMX.Data;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DMX.ViewComponents
{
    public class AddExternalTraining(XContext xContext) : ViewComponent
    {
        public readonly XContext dcx = xContext;

        public IViewComponentResult Invoke()
        {

            AddExternalTrainingVM trainingVM = new ()
            {
                Users = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(dcx.Users.ToList(), "Id", "Fullname"),
            };

            return View(trainingVM);
        }
    }
}

