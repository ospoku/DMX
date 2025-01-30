using AspNetCoreHero.ToastNotification.Abstractions;
using DMX.Data;
using DMX.DataProtection;
using DMX.Models;
using DMX.Services;
using DMX.ViewComponents;
using DMX.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DMX.Controllers
{
   
    public class MemoController(XContext dContext, UserManager<AppUser> userManager, INotyfService notyfService , IAuthorizationService authorizationService, EntityService entityService, AssignmentService assignmentService) : Controller
    {
        public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser> usm = userManager;
        public readonly INotyfService notyf = notyfService;
        public readonly IAuthorizationService auth=authorizationService;
        public readonly EntityService entityServ = entityService;
        public readonly AssignmentService assignmentServ = assignmentService;
        //[HttpPost]
        //public async Task<IActionResult> EditMemo(string Id, EditMemoVM editMemoVM)
        //{
        //    if (editMemoVM.SelectedUsers == null || !editMemoVM.SelectedUsers.Any())
        //    {
        //        notyf.Error("You must select at least one user for assignment.", 5);
        //        return RedirectToAction("ViewMemos");
        //    }

        //    try
        //    {
        //        var decryptedId = Encryption.Decrypt(Id);
        //        var updateThisMemo = await dcx.Memos.FirstOrDefaultAsync(a => a.MemoId == decryptedId);

        //        if (updateThisMemo == null)
        //        {
        //            notyf.Error("Memo not found.", 5);
        //            return RedirectToAction("ViewMemos");
        //        }

        //        // Update memo properties
        //        updateThisMemo.Content = editMemoVM.Content;
        //        updateThisMemo.Title = editMemoVM.Title;

        //        bool IsEdited = await entityServ.EditEntityAsync(updateThisMemo, User);

        //        if (!IsEdited)
        //        {
        //            notyf.Error("Failed to update memo. Please try again.", 5);
        //            return RedirectToAction("ViewMemos");
        //        }

        //        // Remove existing memo assignments
        //        var existingAssignments = dcx.MemoAssignments.Where(x => x.MemoId == decryptedId);
        //        dcx.MemoAssignments.RemoveRange(existingAssignments);

        //        bool allAssignmentsSuccessful = true;
        //        List<string> failedUsers = new List<string>();

        //        foreach (var userId in editMemoVM.SelectedUsers)
        //        {
        //            bool reassign = await assignmentServ.AssignUsers(new MemoAssignment { MemoId = updateThisMemo.MemoId, UserId = userId }, User);
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

        //        return RedirectToAction("ViewMemos");
        //    }
        //    catch (Exception ex)
        //    {
        //        notyf.Error("An unexpected error occurred. Please try again.", 5);
        //        // Log the error (if logging is enabled in your project)
        //        Console.WriteLine($"Error updating memo: {ex.Message}");
        //        return RedirectToAction("Error", "Home", new { message = "An error occurred while processing the memo." });
        //    }
        //}









        [HttpPost]
        public async Task<IActionResult> EditMemo(string Id, EditMemoVM editMemoVM)
        {
            if (editMemoVM.SelectedUsers == null || !editMemoVM.SelectedUsers.Any())
            {
                notyf.Error("You must select at least one user for assignment.", 5);
                return RedirectToAction("ViewMemos");
            }

            try
            {
                var decryptedId = Encryption.Decrypt(Id);
                var updateThisMemo = await dcx.Memos.FirstOrDefaultAsync(a => a.MemoId == decryptedId);

                if (updateThisMemo == null)
                {
                    notyf.Error("Memo not found.", 5);
                    return RedirectToAction("ViewMemos");
                }

                // Update memo properties
                updateThisMemo.Content = editMemoVM.Content;
                updateThisMemo.Title = editMemoVM.Title;

                bool IsEdited = await entityServ.EditEntityAsync(updateThisMemo, User);

                if (!IsEdited)
                {
                    notyf.Error("Failed to update memo. Please try again.", 5);
                    return RedirectToAction("ViewMemos");
                }

                // Remove existing memo assignments
                var existingAssignments = dcx.MemoAssignments.Where(x => x.MemoId == decryptedId);
                dcx.MemoAssignments.RemoveRange(existingAssignments);

                bool atLeastOneFailed = false;
                List<string> failedUsers = new List<string>();

                foreach (var userId in editMemoVM.SelectedUsers)
                {
                    bool reassign = await assignmentServ.AssignUsers(
                        new MemoAssignment { MemoId = updateThisMemo.MemoId, UserId = userId },
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

                return RedirectToAction("ViewMemos");
            }
            catch (Exception ex)
            {
                notyf.Error("An unexpected error occurred. Please try again.", 5);
                Console.WriteLine($"Error updating Memo: {ex.Message}");
                return RedirectToAction("Error", "Home", new { message = "An error occurred while processing the memo." });
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
        [HttpGet]
        public IActionResult AddMemo()
        {
            return ViewComponent("AddMemo");
        }
        [HttpPost]
        public async Task<IActionResult> AddMemo(AddMemoVM addMemoVM)
        {
            if (addMemoVM.SelectedUsers == null || !addMemoVM.SelectedUsers.Any())
            {
                notyf.Error("You must select at least one user for assignment.", 5);
           

                return RedirectToAction("ViewMemos"); // Return the form with the error
            }
            try
            {

    //            var existingMember = await dcx.Memos.FirstOrDefaultAsync(m =>
    //m. == addMemberVM.Name &&
    //m.DoB == addMemberVM.DoB &&
    //m.Telephone == addMemberVM.Telephone);

    //            if (existingMember != null)
    //            {
    //                ModelState.AddModelError("", "A member with the same name, date of birth, and telephone already exists.");
    //                return View(addMemberVM);
    //            }

                // Create the memo object
                Memo addThisMemo = new()
                {
                    Content = addMemoVM.Content,
                    Title = addMemoVM.Title,
                };

                // Attempt to add the memo
                bool result = await entityServ.AddEntityAsync(addThisMemo, User);

                if (result)
                {
                    // If users are selected for assignment
                    if (addMemoVM.SelectedUsers != null && addMemoVM.SelectedUsers.Any())
                    {
                        foreach (var user in addMemoVM.SelectedUsers)
                        {
                            MemoAssignment assignThisMemo = new()
                            {
                                MemoId = addThisMemo.MemoId,
                                UserId = user,
                            };

                            bool assignResult = await assignmentServ.AssignUsers(assignThisMemo, User);

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
                return RedirectToAction("Error","Home", new { message = "An error occurred while processing the memo." });
            }
        }


        [HttpGet]
        public async Task<IActionResult> EditMemoAsync(string Id)
        {
            Memo? memoId = (from x in dcx.Memos where x.MemoId == Encryption.Decrypt(Id) select x).FirstOrDefault();
            if (memoId == null)
            {
                return new NotFoundResult();
            }
            var authorizationResult = await auth.AuthorizeAsync(User, memoId,"MemoOwnerPolicy");
            if (authorizationResult.Succeeded)
            { 
                return ViewComponent("EditMemo", Id);
            }
            else
            {
              notyf.Error("You do not have access to this resource!", 5);
               
                return Json(new { success =false,message= "You do not have access to this resource!" });

            }
        }


        [HttpPost]
        public async Task<IActionResult> CommentMemo(string Id, MemoCommentVM addCommentVM)
        {
            try
            {
                Memo memoToComment = new();
                memoToComment = (from a in dcx.Memos where a.MemoId == @Encryption.Decrypt(Id) select a).FirstOrDefault();

                MemoComment addThisComment = new()
                {
                    MemoId = memoToComment.MemoId,

               UserId=(await usm.GetUserAsync(User)).Id,
                    Message = addCommentVM.NewComment,

                };
                bool result = await entityServ.AddEntityAsync(addThisComment, User);
                if (result)
                {
                    notyf.Success("Comment successfully saved", 5);
                    return RedirectToAction("ViewMemos");
                }
                else
                {
                    notyf.Error("Comment could not be saved!!!", 5);
                    return RedirectToAction("ViewMemos");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { message = "An error occurred while processing the request.", ex.Message });
            }
        }

        [HttpPost]
        public async Task< IActionResult> DeleteMemo(string Id)
        {
            try
            {


                Memo memoToDelete = dcx.Memos.Where(m => m.MemoId == Encryption.Decrypt(Id)).FirstOrDefault();



                bool IsDeleted = await entityServ.DeleteEntityAsync(memoToDelete, User);
                if (IsDeleted)
                {
                    notyf.Success("Record successfully deleted", 5);

                    return RedirectToAction("ViewMemos");
                }
                else
                {
                    notyf.Error("Record could not be deleted!!!", 5);
                    return RedirectToAction("ViewMemos");
                }
            }
      
                            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { message = "An error occurred while processing the request.", ex.Message });
            }
       
        }
        [HttpGet]
        public IActionResult DetailMemo(string Id)
       => ViewComponent("DetailMemo", Id);
    }
}
