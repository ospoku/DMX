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
    public class DeceasedController(XContext dContext, UserManager<AppUser> userManager, INotyfService notyfService, EmailService emailService,
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
        public IActionResult ViewDeceaseds()
        {
            return ViewComponent("ViewDeceaseds");
        }
        public async Task<string> GetUserEmailAsync(string userId)
        {
            var user = await usm.FindByIdAsync(userId);
            return user?.Email;
        }

        [HttpGet]
        public IActionResult AddDeceased() => ViewComponent("AddDeceased");
        [HttpPost]
        public async Task<IActionResult> AddDeceased(AddDeceasedVM addDeceasedVM)
        {
            if (addDeceasedVM.SelectedUsers?.Any() != true)
            {
                notyf.Error("You must select at least one user for assignment.", 5);
                return RedirectToAction("ViewDeceaseds");
            }

            try
            {
                var existingPatient = await  dcx.Deceased.FirstOrDefaultAsync(p =>
    p.Name.ToLower() == addDeceasedVM.Deceased.ToLower() &&
   p.Depositor.ToLower()==addDeceasedVM.Depositor.ToLower());

                if (existingPatient != null)
                {
                    notyf.Error("This record already exists.");
                    return RedirectToAction("ViewDeceaseds");
                }

                // Create and populate the patient object
                var deceased = new Deceased
                {
                    Date = addDeceasedVM.Date,
                    Diagnoses = addDeceasedVM.Diagnoses,
                    Name = addDeceasedVM.Deceased,
                    WardInCharge = addDeceasedVM.WardInCharge,
                    Depositor = addDeceasedVM.Depositor,
                    DepositorAddress = addDeceasedVM.DepositorAddress,
                    TagNo = addDeceasedVM.TagNo,
                    FolderNo = addDeceasedVM.FolderNo,
                    Description = addDeceasedVM.Description,
                    DeceasedTypeId = addDeceasedVM.DeceasedTypeId,
                };

                // Add patient to the database
                var patientAdded = await entityServ.AddEntityAsync(deceased, User);
                if (!patientAdded)
                {
                    notyf.Error("Failed to add patient.", 5);
                    return RedirectToAction("Error", "Home", new { message = "Patient creation failed." });
                }

                // Assign users if any are selected
                if (addDeceasedVM.SelectedUsers?.Any() == true)
                {
                    foreach (var userId in addDeceasedVM.SelectedUsers)
                    {
                        var assignment = new DeceasedAssignment
                        {
                            PatientId = deceased.PatientId,
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

                return RedirectToAction("ViewDeceaseds");
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
        public async Task<IActionResult> DeceasedComment(string Id, MemoCommentVM addCommentVM)
        {
            try {

                Deceased patientToComment = patientToComment = (from a in dcx.Deceased where a.PatientId == Id select a).FirstOrDefault();
                DeceasedComment addThisComment = new()
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
                    return RedirectToAction("ViewDeceaseds");
                }
                else
                {
                    // No users selected for assignment, but patient creation was successful
                    notyf.Error("Could not be saved", 5);
                    return RedirectToAction("ViewDeceaseds");
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
        public IActionResult CommentDeceased(string Id)
        {
            return ViewComponent("CommentDeceased", Id);
        }

    }
}
