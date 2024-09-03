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
          
            var rand = new Random();
            int digit = 5;
            string RefN = "D" + rand.Next((int)Math.Pow(10, digit - 1), (int)Math.Pow(10, digit));

            //if (ModelState.IsValid)
            //{
                Patient addThisPatient = new()
                {
                    Date = addPatientVM.Date,
                    FinalDiagnoses = addPatientVM.FinalDiagnoses,
                    ReferenceNumber = RefN,
                    WardInCharge = addPatientVM.WardInCharge,
                    Depositor=addPatientVM.Depositor,
                    DepositorAddress=addPatientVM.DepositorAddress,
                    TagNo=addPatientVM.TagNo,
                    FolderNo=addPatientVM.FolderNo,
                    Description=addPatientVM.Description,
                    DeceasedTypeId=addPatientVM.DeceasedTypeId,
                    CreatedBy = usm.GetUserAsync(User).Result.UserName,
                    CreatedDate = DateTime.Now,
                };
                dcx.Patients.Add(addThisPatient);

                foreach (var user in addPatientVM.SelectedUsers)
                {

                    dcx.PatientAssignments.Add(new PatientAssignment
                    {
                        PatientId = addThisPatient.PatientId,
                        AppUserId = user,
                        CreatedBy = usm.GetUserAsync(User).Result.UserName,
                        CreatedDate = DateTime.UtcNow,
                    });
                }
                if (await dcx.SaveChangesAsync(usm.GetUserAsync(User).Result.UserName) > 0)
                {
                    notyf.Success("Record successfully saved", 5);

                    return RedirectToAction("ViewPatients");
                }
        






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


                CreatedBy = usm.GetUserAsync(User).Result.UserName,
                  UserId = usm.GetUserAsync(User).Result.Id,
            };

            dcx.PatientComments.Add(addThisComment);
            await dcx.SaveChangesAsync(usm.GetUserAsync(User).Result.UserName);

            return RedirectToAction("ViewMemos");
        }

        [HttpGet]
        public IActionResult PrintPatient(string Id)
        {
            return ViewComponent("PrintPatient", Id);
        }

       
    }
}
