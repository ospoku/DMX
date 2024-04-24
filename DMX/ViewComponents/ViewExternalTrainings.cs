using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using DMX.Data;
using DMX.Models;
using DMX.ViewModels;

namespace DMX.ViewComponents
{
    public class ViewExternalTrainings(XContext dContext, UserManager<AppUser> userManager) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser> usm = userManager;

        public IViewComponentResult Invoke()
        {


            var trainingList = dcx.ExternalTrainings.Where(d => d.IsDeleted == false).Select(d => new ViewExternalTrainingsVM
            {

               TrainingId=d.TrainingId,
                ProposedTrainingDate = d.ProposedTrainingDate,
                NumberofDays = d.NumberofDays,
                DepartureDate = d.DepartureDate,
                WorkshopTitle = d.WorkshopTitle,
                ReturnDate = d.ReturnDate,
                Description=d.Description,
                ProposedTrainingGroup=d.ProposedTrainingGroup,
               
               

            }).ToList();

           
            return View(trainingList);
        }
    }
}
