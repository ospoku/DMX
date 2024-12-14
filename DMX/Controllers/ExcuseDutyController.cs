using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using DMX.Data;
using DMX.DataProtection;
using DMX.Models;
using DMX.Services;

using DMX.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DMX.Controllers
{
    public class ExcuseDutyController(XContext dContext, UserManager<AppUser> userManager, INotyfService notyfService, EntityService entityService, IAuthorizationService authorizationService) : Controller
    {

        public readonly IAuthorizationService auth = authorizationService;
        public readonly UserManager<AppUser> usm = userManager;
        public readonly XContext dcx = dContext;
        private readonly INotyfService notyf = notyfService;
        private readonly EntityService entityServ = entityService;
        [HttpGet]
        public IActionResult ViewExcuseDuties()
        {
            return ViewComponent("ViewExcuseDuties");
        }
        [HttpGet]
        public IActionResult DetailExcuseDuty(string Id)
        {
            return ViewComponent("DetailExcuseDuty", Id);
        }
        [HttpPost]
        public async Task<IActionResult> CommentExcuseDuty(string Id, MemoCommentVM addCommentVM)
        {
            try { 
            ExcuseDuty dutyToComment = new();
            dutyToComment = (from a in dcx.ExcuseDuties where a.Id == Id select a).FirstOrDefault();

                ExcuseDutyComment addThisComment = new()
                {
                    ExcuseDutyId = dutyToComment.Id,


                    Message = addCommentVM.NewComment,



                    UserId = (await usm.GetUserAsync(User)).Id
                };
                bool result = await entityServ.AddEntityAsync(addThisComment, User);
                if (result)
            {
                notyf.Success("Comment successfully saved", 5);
                return RedirectToAction("ViewExcuseDuties");
            }
            else
            {
                notyf.Error("Comment could not be saved!!!", 5);
                return RedirectToAction("ViewExcuseDuties");
            }
        }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { message = "An error occurred while processing the request.", ex.Message
    });
            }
        }
        

        [HttpGet]
        public async Task<IActionResult> EditExcuseDutyAsync(string Id)
        {

            ExcuseDuty? excuseDutyId = (from x in dcx.ExcuseDuties where x.Id == Encryption.Decrypt(Id) select x).FirstOrDefault();
            if (excuseDutyId == null)
            {
                return new NotFoundResult();
            }
            var authorizationResult = await auth.AuthorizeAsync(User, excuseDutyId, "ExcuseDutyOwnerPolicy");
            if (authorizationResult.Succeeded)
            {
                return ViewComponent("EditExcuseDuty", Id);
            }
            else
            {
                notyf.Error("You do not have access to this resource!", 5);

                return Json(new { success = false, message = "You do not have access to this resource!" });

            }
      
        }
        [HttpPost]
        public async Task<IActionResult> AddExcuseDuty(AddExcuseDutyVM addExcuseDutyVM)
        {
            if (addExcuseDutyVM.SelectedUsers == null || !addExcuseDutyVM.SelectedUsers.Any())
            {
                notyf.Error("You must select at least one user for assignment.", 5);


                return RedirectToAction("ViewMemos"); // Return the form with the error
            }
            try
            {


                ExcuseDuty addThisExcuseDuty = new()
                {
                    Date = addExcuseDutyVM.Date,
                    DateofDischarge = addExcuseDutyVM.DateofDischarge,
                    ExcuseDays = addExcuseDutyVM.ExcuseDays,

                    OperationDiagnosis = addExcuseDutyVM.OperationDiagnosis,

                };
                bool result = await entityServ.AddEntityAsync(addThisExcuseDuty, User);
                if (result)
                {
                    try
                    {

                        foreach (var user in addExcuseDutyVM.SelectedUsers)
                        {

                            ExcuseDutyAssignment assignThisExcuseDuty = new ExcuseDutyAssignment()
                            {
                                ExcuseDutyId = addThisExcuseDuty.Id,
                                AppUserId = user,

                            };
                            bool assignResult = await entityServ.AddEntityAsync(assignThisExcuseDuty, User);
                            if (assignResult)
                            {
                                notyf.Success("Record successfully saved", 5);
                                return RedirectToAction("ViewExcuseDuties");
                            }
                            else
                            {
                                notyf.Error("Error, record could not be saved.", 5);
                                return RedirectToAction("ViewExcuseDuties");
                            }
                        }
                    }
                    catch (Exception innerEx) 
                    {
                        notyf.Error("Error processing users: " + innerEx.Message, 5);
                        return RedirectToAction("ErrorPage", new { message = "An error occurred while processing users." });
                            }



                            return RedirectToAction("ErrorPage", new { message = "Failed to add the memo. Please try again." });
        }
                        else
                        {
                            // Failure: Handle error and redirect to a setup or error page
                            notyf.Error("Memo creation failed.", 5);
                            return RedirectToAction("ErrorPage", new { message = "Failed to add the memo. Please try again." });
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log the error (optional)
                        notyf.Error("An error occurred: " + ex.Message, 5);
// Redirect to an error page with a custom error message
return RedirectToAction("ErrorPage", new { message = ex.Message });

                    }
                }



                [HttpPost]
        public async Task<IActionResult> EditExcuseDutyAsync(string Id, EditExcuseDutyVM editExcuseDutyVM)
        {

            ExcuseDuty updateThisExcuseDuty = dcx.ExcuseDuties.Where(e => e.Id == @Encryption.Decrypt(Id)).Select(e => e).FirstOrDefault();


           
            updateThisExcuseDuty.DateofDischarge = editExcuseDutyVM.DateofDischarge;
            updateThisExcuseDuty.ExcuseDays = editExcuseDutyVM.ExcuseDays;

            updateThisExcuseDuty.OperationDiagnosis = editExcuseDutyVM.OperationDiagnosis;

            updateThisExcuseDuty.ModifiedBy = User.Claims.FirstOrDefault(c => c.Type == "Name").Value;


            foreach (var assignment in dcx.MemoAssignments.Where(a => a.MemoId == @Encryption.Decrypt(Id)))
            {
                dcx.MemoAssignments.Remove(assignment);
            };
            dcx.SaveChanges();
            dcx.ExcuseDuties.Attach(updateThisExcuseDuty);

            dcx.Entry(updateThisExcuseDuty).State = EntityState.Modified;

            foreach (var user in editExcuseDutyVM.SelectedUsers)
            {
               dcx.ExcuseDutyAssignments.Add(new ExcuseDutyAssignment
               {
                    ExcuseDutyId = @Encryption.Decrypt(Id),
                    AppUserId = user,
                   ModifiedDate = DateTime.Now,
               });
            }
            if (await dcx.SaveChangesAsync(User.Claims.FirstOrDefault(c => c.Type == "Name").Value) > 0)
            {
                notyf.Success("Record successfully updated");

                return RedirectToAction("ViewExcuseDuties");
            }
            else
            {
                return ViewComponent("EditExcuseDuty");
            }

        }

        [HttpGet]
        public IActionResult AddExcuseDuty()
        {
            return ViewComponent("AddExcuseDuty");
        }



    }
}
