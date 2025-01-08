using AspNetCoreHero.ToastNotification.Abstractions;
using DMX.Data;
using DMX.Models;
using DMX.Services;
using DMX.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DMX.Controllers
{
    public class SickReportController (XContext dContext, UserManager<AppUser> userManager, INotyfService notyfService, IAuthorizationService authorizationService, EntityService entityService, AssignmentService assignmentService) : Controller
    {
        public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser> usm = userManager;
        public readonly INotyfService notyf = notyfService;
        public readonly IAuthorizationService auth = authorizationService;
        public readonly EntityService entityServ = entityService;
        public readonly AssignmentService assignmentServ = assignmentService;

    




        [HttpGet]
        public IActionResult AddSickReport() => ViewComponent("AddSickReport");

        [HttpGet]
        public IActionResult ViewSickReports() => ViewComponent("ViewSickReports");
        [HttpPost]
        public async Task<IActionResult> AddSickReport(AddSickReportVM addSickReportVM)
        {
            if (addSickReportVM.SelectedUsers == null || !addSickReportVM.SelectedUsers.Any())
            {
                notyf.Error("You must select at least one user for assignment.", 5);


                return RedirectToAction("ViewMemos"); // Return the form with the error
            }
            try
            {
                // Create the memo object
                SickReport addThisSickReport = new()
                {
                   AdditionalNotes=addSickReportVM.AdditionalNotes
                };

                // Attempt to add the memo
                bool result = await entityServ.AddEntityAsync(addThisSickReport, User);

                if (result)
                {
                    // If users are selected for assignment
                    if (addSickReportVM.SelectedUsers != null && addSickReportVM.SelectedUsers.Any())
                    {
                        foreach (var user in addSickReportVM.SelectedUsers)
                        {
                            SickAssignment assignThisSickReport = new()
                            {
                                SickId = addThisSickReport.SickReportId,
                                AppUserId=user,
                            };

                            bool assignResult = await assignmentServ.AssignUsers(assignThisSickReport, User);

                            if (!assignResult)
                            {
                                notyf.Error($"Failed to assign memo to user {user}.", 5);
                                // Continue processing other users, but log the failure


                            }
                        }
                    }








                    // If everything is processed successfully
                    notyf.Success("Memo and assignments successfully processed.", 5);
                    return RedirectToAction("ViewMemos");
                }
                else
                {
                    // Failed to add the memo
                    notyf.Error("Failed to add the memo. Please try again.", 5);
                    return RedirectToAction("ViewMemos");
                }
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                notyf.Error("An error occurred: " + ex.Message, 5);
                return RedirectToAction("Error", "Home", new { message = "An error occurred while processing the memo." });
            }
        }


    }
}
