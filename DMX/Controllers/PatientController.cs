using AspNetCoreHero.ToastNotification.Abstractions;
using DMX.Data;
using DMX.Models;
using DMX.Services;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMX.Controllers
{
    public class PatientController(XContext dContext, UserManager<AppUser> userManager, INotyfService notyfService,
          EntityService entityService) : Controller
    {
        public readonly UserManager<AppUser> usm = userManager;
        public readonly XContext dcx = dContext;
        private readonly INotyfService notyf = notyfService;
        public readonly EntityService entityServ = entityService;



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
           if (addPatientVM.SelectedUsers == null || !addPatientVM.SelectedUsers.Any())
            {
                notyf.Error("You must select at least one user for assignment.",5);
                // Optionally, repopulate the view model and return the form to the user
                //addPatientVM.UsersList = userService.GetAllUsers().Select(u => new SelectListItem { Value = u.Id, Text = u.Name }).ToList();
               
                return  RedirectToAction ("ViewPatients"); // Return the form with the error
            }
            try
            {
                // Create the patient object
                Patient addThisPatient = new()
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
                // Attempt to add the patient
                bool result = await entityServ.AddEntityAsync(addThisPatient, User);
                if (result)
                {
                    // If users are selected for assignment
                    if (addPatientVM.SelectedUsers != null && addPatientVM.SelectedUsers.Any())
                    {
                        // Process user assignments
                        foreach (var user in addPatientVM.SelectedUsers)
                        {
                            PatientAssignment addpatientAssignment = new()
                            {
                                PatientId = addThisPatient.PatientId,
                                AppUserId = user,
                            };

                            bool patientAssign = await entityServ.AddEntityAsync(addpatientAssignment, User);

                            if (!patientAssign)
                            {
                                notyf.Error("Failed to assign user.", 5);
                                return RedirectToAction ("Error","Home", new { message = "An error occurred while assigning users." });
                            }
                            else
                            {
                                Hangfire.BackgroundJob.Enqueue<NotificationService>(notificationService =>notificationService.SendEmail(addpatientAssignment.AppUser.Email, "New Patient Assignment", $"You have been assigned a new patient: {addpatientAssignment}"));

                                Hangfire.BackgroundJob.Enqueue<NotificationService>(notificationService =>
                                    notificationService.SendSMS(addpatientAssignment.AppUser.PhoneNumber, $"You have a new memo assignment: {addpatientAssignment}"));
                            }
                        }
                        // If all assignments are successful
                        notyf.Success("Record and assignments successfully processed.", 5);
                        return RedirectToAction("ViewPatients");
                    }
                    else
                    {
                        // No users selected for assignment, but patient creation was successful
                        notyf.Success("Patient successfully created, no users to assign.", 5);
                        return RedirectToAction("ViewPatients");
                    }
                }
                else
                {
                    // Failed to add patient
                    notyf.Error("Failed to add patient.", 5);
                    return RedirectToAction("Error","Home", new { message = "An error occurred while processing the patient." });
                }
            }
            catch
            {
                // General catch for any unexpected exceptions
                notyf.Error("An error occurred while processing the request.", 5);
                return RedirectToAction("ErrorPage", new { message = "An error occurred while processing the request." });
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
