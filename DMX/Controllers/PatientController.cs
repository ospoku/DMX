using AspNetCoreHero.ToastNotification.Abstractions;
using DMX.Data;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DMX.Controllers
{
    public class PatientController (XContext dContext, UserManager<AppUser> userManager, INotyfService notyfService
           ) : Controller
    {
        public readonly UserManager<AppUser> usm = userManager;
        public readonly XContext dcx = dContext;
        private readonly INotyfService notyf = notyfService;



        [HttpGet]
        public IActionResult EditPatient(string Id)
        {
            return ViewComponent("EditPatient", Id);
        }
        public IActionResult ViewPatients()
        {
            return ViewComponent("ViewPatients");
        }
        [HttpPost]
        public async Task<IActionResult> PatientComment(string Id, MemoCommentVM addCommentVM)
        {

            Patient patientToComment = new();
            patientToComment = (from a in dcx.Patients where a.PatientId == Id select a).FirstOrDefault();

            PatientComment addThisComment = new()
            {
                PatientId =patientToComment.PatientId,
                CreatedDate = DateTime.Now,

                Message = addCommentVM.NewComment,


                CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Name").Value,
                  UserId = usm.FindByNameAsync(User.Claims.FirstOrDefault(c => c.Type == "Name").Value).Result.Id,
            };

            dcx.PatientComments.Add(addThisComment);
            await dcx.SaveChangesAsync();

            return RedirectToAction("ViewMemos");
        }


    }
}
