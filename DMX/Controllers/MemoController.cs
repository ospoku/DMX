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

        [HttpPost]
        public async Task<IActionResult> EditMemo(string Id, EditMemoVM editMemoVM)
        {
            // Decrypt the memo ID and fetch the memo to update
            var decryptedId = Encryption.Decrypt(Id);
            var updateThisMemo = await dcx.Memos.FirstOrDefaultAsync(a => a.MemoId == decryptedId);

            if (updateThisMemo == null)
            {
                // Handle the case where the memo is not found
                return NotFound();
            }

            // Update memo properties
            updateThisMemo.Content = editMemoVM.Content;
            updateThisMemo.Title = editMemoVM.Title;
            updateThisMemo.ModifiedDate = DateTime.UtcNow;

            var currentUser = await usm.GetUserAsync(User);
            updateThisMemo.ModifiedBy = currentUser?.UserName;

            // Mark the entity as modified
            dcx.Entry(updateThisMemo).State = EntityState.Modified;

            // Remove existing memo assignments
            var existingAssignments = dcx.MemoAssignments.Where(x => x.MemoId == decryptedId);
            dcx.MemoAssignments.RemoveRange(existingAssignments);

            // Add new memo assignments
            foreach (var userId in editMemoVM.SelectedUsers)
            {
                dcx.MemoAssignments.Add(new MemoAssignment
                {
                    MemoId = updateThisMemo.MemoId,
                    AppUserId = userId,
                    CreatedBy = currentUser?.UserName,
                    CreatedDate = DateTime.UtcNow,
                });
            }

            // Save changes to the database
            var changes = await dcx.SaveChangesAsync(currentUser?.UserName);

            if (changes > 0)
            {
                notyf.Success("Record successfully saved", 5);
                return RedirectToAction("ViewMemos");
            }

            // If saving fails, show the edit memo view with error
            return ViewComponent("EditMemo");
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

            var breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem{Title="Home", Url="/"},
                new BreadcrumbItem{Title="Memos", Url=@Url.Action("ViewMemos")}
            };

            ViewBag.BreadcrumbItems = breadcrumbs;

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
            try
            {
                Memo addThisMemo = new()
                {
                Content = addMemoVM.Content,
                Title = addMemoVM.Title,
                };
                bool result = await entityServ.AddEntityAsync(addThisMemo, User);
                if (result)
                {
                    try
                    {
                        foreach (var user in addMemoVM.SelectedUsers)
                        {

                            MemoAssignment assignThisMemo = new MemoAssignment()
                            {
                                MemoId = addThisMemo.MemoId,
                                AppUserId = user,

                            };
                            bool assignResult = await assignmentServ.AssignUsers(assignThisMemo, User);

                            if (assignResult)
                            {
                                return RedirectToAction("ViewMemos");
                            }
                            else
                            {

                                return RedirectToAction("ViewMemos");
                            }
                        }
                  

                     
                    }
                    catch
                    {

                    }
                    return RedirectToAction("SystemSetup");
                }

                // Success: Redirect to SystemSetup
                else
                {
                    // Failure: Return an error view or handle as needed
                    return RedirectToPage  ("/ErrorPage", new { message = "Failed to add the memo. Please try again." });
                }
            }
            catch (Exception ex)
            {
                notyf.Error("An error occurred: " + ex.Message, 5);
                return RedirectToAction("ViewMemos");
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> AddMemos(AddMemoVM addMemoVM)
        //{
        //    try
        //    {
        //        // Create a new memo object from the ViewModel
        //        Memo addThisMemo = new()
        //        {
        //            Content = addMemoVM.Content,
        //            Title = addMemoVM.Title,
        //        };

        //        // Add the memo to the database using an asynchronous method
        //        bool result = await entityServ.AddEntityAsync(addThisMemo, User);

        //        if (result)
        //        {
        //            // Success: Loop through the selected users and perform any necessary actions
        //            try
        //            {
        //                foreach (var user in addMemoVM.SelectedUsers)
        //                {
        //                    // Perform actions for each user (e.g., send notifications)
        //                    // ... your logic here
        //                }
        //            }
        //            catch (Exception innerEx)
        //            {
        //                // Log the error (optional)
        //                notyf.Error("Error processing users: " + innerEx.Message, 5);
        //                // Redirect to an error page or handle it accordingly
        //                return RedirectToAction("ErrorPage", new { message = "An error occurred while processing users." });
        //            }

        //            // On successful addition and user processing, redirect to SystemSetup
        //            return RedirectToAction("ErrorPage", new { message = "Failed to add the memo. Please try again." });
        //        }
        //        else
        //        {
        //            // Failure: Handle error and redirect to a setup or error page
        //            notyf.Error("Memo creation failed.", 5);
        //            return RedirectToAction("ErrorPage", new { message = "Failed to add the memo. Please try again." });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the error (optional)
        //        notyf.Error("An error occurred: " + ex.Message, 5);
        //        // Redirect to an error page with a custom error message
        //        return RedirectToAction("ErrorPage", new { message = ex.Message });

        //    }
        //}


        [HttpGet]

        
        public async Task<IActionResult> EditMemoAsync(string Id)
        {
            Memo? memoId = (from x in dcx.Memos where x.MemoId == Encryption.Decrypt(Id) select x).FirstOrDefault();
            if (memoId == null)
            {
                return new NotFoundResult();
            }
            var authorizationResult = await auth
           .AuthorizeAsync(User, memoId, "UserOwnsDocumentPolicy");
            if (authorizationResult.Succeeded)
            { 
                return ViewComponent("EditMemo", Id);
            }
            else
            {
                notyf.Error("You do not have access to this resource!!!", 5);
                return ViewComponent("ViewMemos");

            }
        }


        [HttpPost]
        public async Task<IActionResult> CommentMemo(string Id, MemoCommentVM addCommentVM)
        {

            Memo memoToUpdate = new();
            memoToUpdate = (from a in dcx.Memos where a.MemoId == @Encryption.Decrypt(Id) select a).FirstOrDefault();

            MemoComment addThisComment = new()
            {
                MemoId = memoToUpdate.MemoId,

                CreatedDate = DateTime.Now,
                Message = addCommentVM.NewComment,
                CreatedBy = usm.GetUserAsync(User).Result.UserName,
                UserId = usm.GetUserAsync(User).Result.Id,

            };

            dcx.MemoComments.Add(addThisComment);
            if (await dcx.SaveChangesAsync(usm.GetUserAsync(User).Result.UserName) > 0)
            {

                return RedirectToAction("ViewMemos");
            }
            else
            {
                notyf.Error("Error, Record could not be saved!!!", 5);
                return RedirectToAction("ViewMemos");
            }
        }

        [HttpPost]
        public async Task< IActionResult> DeleteMemo(string Id)
        {
            var memoToDelete = (from m in dcx.Memos where m.MemoId == Encryption.Decrypt(Id) select m).FirstOrDefault();


            memoToDelete.IsDeleted = true;
            dcx.Memos.Attach(memoToDelete);
            dcx.SaveChanges();

            return ViewComponent("ViewMemos");
        }
        [HttpGet]
        public IActionResult DetailMemo(string Id)
       => ViewComponent("DetailMemo", Id);
    }
}
