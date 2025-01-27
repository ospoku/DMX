using AspNetCoreHero.ToastNotification.Abstractions;
using DMX.Data;
using DMX.DataProtection;
using DMX.Models;
using DMX.Services;

using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DMX.Controllers
{
    public class PettyCashController(XContext dContext, UserManager<AppUser> userManager, INotyfService notyfService, EntityService entityService
          ) : Controller
    {

        public readonly UserManager<AppUser> usm = userManager;
        public readonly XContext dcx = dContext;
        private readonly INotyfService notyf = notyfService;

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
                            AppUserId = user,

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
                    return RedirectToAction("ViewMemos");
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
        
        

        [HttpPost]
        public async Task<IActionResult> EditPettyCash()
        {
            return  RedirectToAction("ViewPettyCash");
        }

    }
}
