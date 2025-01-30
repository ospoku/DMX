using AspNetCoreHero.ToastNotification.Abstractions;
using DMX.Data;
using DMX.DataProtection;
using DMX.Models;
using DMX.Services;

using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DMX.Controllers
{
    public class PettyCashController(XContext dContext, UserManager<AppUser> userManager, INotyfService notyfService, EntityService entityService, AssignmentService assignmentService
          ) : Controller
    {

        public readonly UserManager<AppUser> usm = userManager;
        public readonly XContext dcx = dContext;
        private readonly INotyfService notyf = notyfService;
        public readonly AssignmentService assignmentServ = assignmentService;
        private readonly EntityService entityServ = entityService;

        [HttpGet]
        public IActionResult EditPettyCash(string Id)
        {
            return ViewComponent("EditPettyCash", Id);
        }

        [HttpGet]
        public IActionResult ViewPettyCash()
        {
            return ViewComponent("ViewPettyCash");
        }
        [HttpPost]
        public async Task<IActionResult> AddPettyCash(AddPettyCashVM addPettyCashVM)
        {
            if (addPettyCashVM.SelectedUsers == null || !addPettyCashVM.SelectedUsers.Any())
            {
                notyf.Error("You must select at least one user for assignment.", 5);
                // Optionally, repopulate the view model and return the form to the user
                //addPatientVM.UsersList = userService.GetAllUsers().Select(u => new SelectListItem { Value = u.Id, Text = u.Name }).ToList();

                return RedirectToAction("ViewPettyCash"); // Return the form with the error
            }
            try
            {


                PettyCash addThisPettyCash = new()
                {
                    Amount = addPettyCashVM.Amount,
                    Purpose = addPettyCashVM.Purpose,

                };
                bool result = await entityServ.AddEntityAsync(addThisPettyCash, User);
                if (result)
                {
                    foreach (var user in addPettyCashVM.SelectedUsers)
                    {
                        PettyCashAssignment cashAssignment = new PettyCashAssignment()
                        {
                            PettyCashId = addThisPettyCash.PettyCashId,
                            UserId = user,
                        };
                        bool assignPettyCash = await entityServ.AddEntityAsync(cashAssignment, User);
                        if (assignPettyCash)
                        {
                            notyf.Success("PettyCash and assignments successfully processed", 5); return RedirectToAction("ViewPettyCash");
                        }
                        else
                        {
                            notyf.Error("Error, record could not be saved.", 5);
                            return RedirectToAction("ViewPettyCash");
                        }
                        // If everything is processed successfully
                    }
                    notyf.Success("PettyCash and assignments successfully processed.", 5);
                    return RedirectToAction("ViewPettyCash");
                }
                else
                {
                    notyf.Error("An error occurred while processing the request.", 5);
                    return RedirectToAction("Error", "Home", new { message = "An error occurred while processing the request." });
                }
            }
            catch (Exception ex)
            {
                notyf.Error("An error occurred while processing the request.", 5);
                return RedirectToAction("ViewPettyCash");
            }
        }


        [HttpGet]
        public IActionResult AddPettyCash()
        {
           
                return ViewComponent("AddPettyCash");
            
        }


        [HttpGet]
        public IActionResult CommentPettyCash(string Id)
        {
            return ViewComponent("CommentPettyCash", Id);
        }

        [HttpPost]
        public async Task<IActionResult> CommentPettyCash(string Id, PettyCashCommentVM addCommentVM)
        {
            try
            {
                PettyCash pettycashToComment = new();
                pettycashToComment = (from a in dcx.PettyCash where a.PettyCashId == Encryption.Decrypt(Id) select a).FirstOrDefault();

                PettyCashComment addThisComment = new()
                {
                    PettyCashId = pettycashToComment.PettyCashId,
                    CreatedDate = DateTime.Now,

                    Message = addCommentVM.NewComment,

                    UserId = (await usm.GetUserAsync(User)).Id,
                };

                bool result = await entityServ.AddEntityAsync(addThisComment, User);
                if (result)
                {
                    notyf.Success("Comment successfully saved", 5);
                    return RedirectToAction("ViewPettyCash");
                }
                else
                {
                    notyf.Error("Comment could not be saved!!!", 5);
                    return RedirectToAction("ViewPettyCash");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new
                {
                    message = "An error occurred while processing the request.",
                    ex.Message
                });
            }
        }



        //[HttpPost]
        //public async Task<IActionResult> EditPettyCash(EditPettyCashVM editPettyCashVM, string Id)
        //{
        //    if (editPettyCashVM.SelectedUsers == null || !editPettyCashVM.SelectedUsers.Any())
        //    {
        //        notyf.Error("You must select at least one user for assignment.", 5);
        //        return RedirectToAction("ViewPettyCash");
        //    }

        //    try
        //    {
        //        var decryptedId = Encryption.Decrypt(Id);
        //        var updateThisPettyCash = await dcx.PettyCash.FirstOrDefaultAsync(a => a.PettyCashId == decryptedId);

        //        if (updateThisPettyCash == null)
        //        {
        //            notyf.Error("Petty Cash record not found.", 5);
        //            return RedirectToAction("ViewPettyCash");
        //        }

        //        // Update properties
        //        updateThisPettyCash.Purpose = editPettyCashVM.Purpose;
        //        updateThisPettyCash.Amount = editPettyCashVM.Amount;

        //        bool IsEdited = await entityServ.EditEntityAsync(updateThisPettyCash, User);

        //        if (!IsEdited)
        //        {
        //            notyf.Error("Failed to update Petty Cash record. Please try again.", 5);
        //            return RedirectToAction("ViewPettyCash");
        //        }

        //        // Remove existing assignments
        //        var existingAssignments = dcx.PettyCashAssignments.Where(x => x.Id == decryptedId);
        //        dcx.PettyCashAssignments.RemoveRange(existingAssignments);

        //        bool allAssignmentsSuccessful = true;
        //        List<string> failedUsers = new List<string>();

        //        foreach (var userId in editPettyCashVM.SelectedUsers)
        //        {
        //            bool reassign = await assignmentServ.AssignUsers(
        //                new PettyCashAssignment { Id = updateThisPettyCash.PettyCashId, UserId = userId },
        //                User
        //            );

        //            if (!reassign)
        //            {
        //                allAssignmentsSuccessful = false;
        //                failedUsers.Add(userId);
        //            }
        //        }

        //        if (allAssignmentsSuccessful)
        //        {
        //            notyf.Success("Record successfully updated", 5);
        //        }
        //        else
        //        {
        //            notyf.Warning($"Record updated, but some assignments failed: {string.Join(", ", failedUsers)}", 7);
        //        }

        //        return RedirectToAction("ViewPettyCash");
        //    }
        //    catch (Exception ex)
        //    {
        //        notyf.Error("An unexpected error occurred. Please try again.", 5);
        //        // Log error (you can replace with a proper logging system)
        //        Console.WriteLine($"Error updating Petty Cash: {ex.Message}");
        //        return RedirectToAction("Error", "Home", new { message = "An error occurred while processing the Petty Cash record." });
        //    }
        //}









        [HttpPost]
        public async Task<IActionResult> EditPettyCash(EditPettyCashVM editPettyCashVM, string Id)
        {
            if (editPettyCashVM.SelectedUsers == null || !editPettyCashVM.SelectedUsers.Any())
            {
                notyf.Error("You must select at least one user for assignment.", 5);
                return RedirectToAction("ViewPettyCash");
            }

            try
            {
                var decryptedId = Encryption.Decrypt(Id);
                var updateThisPettyCash = await dcx.PettyCash.FirstOrDefaultAsync(a => a.PettyCashId == decryptedId);

                if (updateThisPettyCash == null)
                {
                    notyf.Error("Petty Cash record not found.", 5);
                    return RedirectToAction("ViewPettyCash");
                }

                // Update properties
                updateThisPettyCash.Purpose = editPettyCashVM.Purpose;
                updateThisPettyCash.Amount = editPettyCashVM.Amount;

                bool IsEdited = await entityServ.EditEntityAsync(updateThisPettyCash, User);

                if (!IsEdited)
                {
                    notyf.Error("Failed to update Petty Cash record. Please try again.", 5);
                    return RedirectToAction("ViewPettyCash");
                }

                // Remove existing assignments
                var existingAssignments = dcx.PettyCashAssignments.Where(x => x.PettyCashId == decryptedId);
                dcx.PettyCashAssignments.RemoveRange(existingAssignments);

                bool allAssignmentsSuccessful = true;
                bool atLeastOneFailed = false;
                List<string> failedUsers = new List<string>();

                foreach (var userId in editPettyCashVM.SelectedUsers)
                {
                    bool reassign = await assignmentServ.AssignUsers(
                        new PettyCashAssignment { PettyCashId = updateThisPettyCash.PettyCashId, UserId = userId },
                        User
                    );

                    if (!reassign)
                    {
                        atLeastOneFailed = true;
                        failedUsers.Add(userId);
                    }
                }

                // Display only ONE message
                if (atLeastOneFailed)
                {
                    notyf.Warning($"Record updated, but some assignments failed: {string.Join(", ", failedUsers)}", 7);
                }
                else
                {
                    notyf.Success("Record successfully updated", 5);
                }

                return RedirectToAction("ViewPettyCash");
            }
            catch (Exception ex)
            {
                notyf.Error("An unexpected error occurred. Please try again.", 5);
                // Log error (you can replace with a proper logging system)
                Console.WriteLine($"Error updating Petty Cash: {ex.Message}");
                return RedirectToAction("Error", "Home", new { message = "An error occurred while processing the Petty Cash record." });
            }
        }





        [HttpGet]
        public IActionResult CommentMemo(string Id)
        {
            return ViewComponent("CommentMemo", Id);
        }

        [HttpGet]
        public IActionResult PrintMemo(string Id)
        {
            return ViewComponent("PrintMemo", Id);
        }
        public IActionResult ViewMemos()
        {

            //var breadcrumbs = new List<BreadcrumbItem>
            //{
            //    new BreadcrumbItem{Title="Home", Url="/"},
            //    new BreadcrumbItem{Title="Memos", Url=@Url.Action("ViewMemos")}
            //};

            //ViewBag.BreadcrumbItems = breadcrumbs;

            return ViewComponent("ViewMemos");
        }
    }

    }

