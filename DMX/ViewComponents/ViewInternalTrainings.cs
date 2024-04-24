using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using DMX.Data;
using DMX.Models;
using DMX.ViewModels;

namespace DMX.ViewComponents
{
    public class ViewInternalTrainings(XContext dContext, UserManager<AppUser> userManager) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser> usm = userManager;

        public IViewComponentResult Invoke()
        {


            var trainingList = dcx.InternalTrainings.Where(d => d.IsDeleted == false).Select(d => new ViewInternalTrainingsVM
            {

               TrainingId = d.TrainingId,
                Date = d.Date,
              Description = d.Description,  
               
               EventName = d.EventName,
              

            }).ToList();

           
            return View(trainingList);
        }
    }
}
