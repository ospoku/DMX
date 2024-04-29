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

        [HttpGet]
        public IActionResult AddPatient() => ViewComponent("AddPatient");
        [HttpPost]
        public async Task<IActionResult> AddPatient(AddPatientVM addPatientVM)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Document addition error!!! Please try again";

                return ViewComponent("AddPatient");
            }

            if (ModelState.IsValid)
            {
                Patient addThisPatient = new()
                {
                    Date = addPatientVM.Date,
                    FinalDiagnoses = addPatientVM.FinalDiagnoses,

                    WardInCharge = addPatientVM.WardInCharge,

                    IsDeleted = false,
                    CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Name").Value,
                    CreatedDate = DateTime.Now,
                };
                dcx.Patients.Add(addThisPatient);

                dcx.MemoAssignments.Add(new MemoAssignment
                {
                    // SelectedUsers = string.Join(',', addPatientVM.SelectedUsers),

                });


                if (await dcx.SaveChangesAsync(userId: User?.FindFirst(c => c.Type == "Name").Value) > 0)
                {
                    notyf.Success("Client successfully created.");
                    return RedirectToAction("ViewPatients");

                }
                else
                {
                    notyf.Error("Member creation error!!! Please try again");
                }
                return RedirectToAction("AddPatient");

            }
            else
            {
                return RedirectToAction("AddPatient");
            }


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
