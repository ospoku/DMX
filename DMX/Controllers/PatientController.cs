using AspNetCoreHero.ToastNotification.Abstractions;
using DMX.Data;
using DMX.Models;
using DMX.Services;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DMX.Controllers
{
    public class PatientController(XContext dContext, UserManager<AppUser> userManager, INotyfService notyfService, EmailService emailService,
          EntityService entityService) : Controller
    {
        public readonly UserManager<AppUser> usm = userManager;
        public readonly XContext dcx = dContext;
        private readonly INotyfService notyf = notyfService;
        public readonly EntityService entityServ = entityService;
        public readonly EmailService email = emailService;



        [HttpGet]
        public IActionResult EditPatient(string Id)
        {
            return ViewComponent("EditPatient", Id);
        }
        public IActionResult ViewPatients()
        {
            return ViewComponent("ViewPatients");
        }
        public async Task<string> GetUserEmailAsync(string userId)
        {
            var user = await usm.FindByIdAsync(userId);
            return user?.Email;
        }

        [HttpGet]
        public IActionResult AddPatient() => ViewComponent("AddPatient");
        [HttpPost]
        public async Task<IActionResult> AddPatient(AddPatientVM addPatientVM)
        {
            if (addPatientVM.SelectedUsers?.Any() != true)
            {
                notyf.Error("You must select at least one user for assignment.", 5);
                return RedirectToAction("ViewPatients");
            }

            try
            {
                var existingPatient = await  dcx.Patients.FirstOrDefaultAsync(p =>
    p.Name.ToLower() == addPatientVM.Deceased.ToLower() &&
   p.Depositor.ToLower()==addPatientVM.Depositor.ToLower());

                if (existingPatient != null)
                {
                    ModelState.AddModelError("", "A member with the same name, date of birth, and telephone already exists.");
                    return View(addPatientVM);
                }

                // Create and populate the patient object
                var patient = new Patient
                {
                    Date = addPatientVM.Date,
                    Diagnoses = addPatientVM.Diagnoses,
                    Name = addPatientVM.Deceased,
                    WardInCharge = addPatientVM.WardInCharge,
                    Depositor = addPatientVM.Depositor,
                    DepositorAddress = addPatientVM.DepositorAddress,
                    TagNo = addPatientVM.TagNo,
                    FolderNo = addPatientVM.FolderNo,
                    Description = addPatientVM.Description,
                    DeceasedTypeId = addPatientVM.DeceasedTypeId,
                };

                // Add patient to the database
                var patientAdded = await entityServ.AddEntityAsync(patient, User);
                if (!patientAdded)
                {
                    notyf.Error("Failed to add patient.", 5);
                    return RedirectToAction("Error", "Home", new { message = "Patient creation failed." });
                }

                // Assign users if any are selected
                if (addPatientVM.SelectedUsers?.Any() == true)
                {
                    foreach (var userId in addPatientVM.SelectedUsers)
                    {
                        var assignment = new PatientAssignment
                        {
                            PatientId = patient.PatientId,
                            AppUserId = userId,
                        };

                        var assignmentAdded = await entityServ.AddEntityAsync(assignment, User);
                        if (!assignmentAdded)
                        {
                            notyf.Error("Failed to assign user.", 5);
                            return RedirectToAction("Error", "Home", new { message = "User assignment failed." });
                        }
                    }
                    notyf.Success("Patient and assignments successfully processed.", 5);
                }
                else
                {
                    notyf.Success("Patient created successfully, no users assigned.", 5);
                }

                return RedirectToAction("ViewPatients");
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.Error.WriteLine(ex);
                notyf.Error("An unexpected error occurred.", 5);
                return RedirectToAction("Error", "Home", new { message = "An unexpected error occurred." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> PatientComment(string Id, MemoCommentVM addCommentVM)
        {
            try {

                Patient patientToComment = patientToComment = (from a in dcx.Patients where a.PatientId == Id select a).FirstOrDefault();
                PatientComment addThisComment = new()
                {
                    PatientId = patientToComment.PatientId,
                    Message = addCommentVM.NewComment,
                    UserId = (await usm.GetUserAsync(User)).Id,
                };
                bool result = await entityServ.AddEntityAsync(addThisComment, User);
                if (result)
                {
                    // If all assignments are successful
                    notyf.Success("Comment successfully saved.", 5);
                    return RedirectToAction("ViewPatients");
                }
                else
                {
                    // No users selected for assignment, but patient creation was successful
                    notyf.Error("Could not be saved", 5);
                    return RedirectToAction("ViewPatients");
                }
            }
            catch
            {
    // General catch for any unexpected exceptions
    notyf.Error("An error occurred while processing the request.", 5);
    return RedirectToAction("ErrorPage", new { message = "An error occurred while processing the request." });
}
        }
        [HttpGet]
        public IActionResult PrintPatient(string Id)
        {
            return ViewComponent("PrintPatient", Id);
        }
        [HttpGet]
        public IActionResult CommentPatient(string Id)
        {
            return ViewComponent("CommentPatient", Id);
        }

    }
}
