using AspNetCoreHero.ToastNotification.Abstractions;
using DMX.Data;
using DMX.Models;
using DMX.Services;
using DMX.ViewComponents;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DMX.Controllers
{
    public class PettyCashController(XContext dContext, UserManager<AppUser> userManager, INotyfService notyfService, EntityService entityService
          ) :Controller
    {

        public readonly UserManager<AppUser> usm = userManager;
        public readonly XContext dcx = dContext;
        private readonly INotyfService notyf = notyfService;

        private readonly EntityService entityServ= entityService;

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

                    Date = addPettyCashVM.Date,
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
                        bool assignPettyCash = await entityServ.AddEntityAsync(addThisPettyCash, User);
                    }
                    // If everything is processed successfully
                    notyf.Success("PettyCash and assignments successfully processed.", 5);
                    return RedirectToAction("ViewPettyCash");
                }
                else
                {
                    // Failed to add the memo
                    notyf.Error("Failed to add the Pettycash. Please try again.", 5);
                    return RedirectToAction("ViewPettyCash");
                }
            }
            catch (Exception ex)
            {
                notyf.Error("An error occurred while processing the request.", 5);
                return RedirectToAction("Error","Home", new { message = "An error occurred while processing the request." });
            }
           
          
            
        }


        [HttpGet]
        public IActionResult AddPettyCash()
        {
           
                return ViewComponent("AddPettyCash");
            
        }

        [HttpPost]
        public async Task<IActionResult> AddPettyCashComment(string Id, MemoCommentVM addCommentVM)
        {

            Memo memoToUpdate = new();
            memoToUpdate = (from a in dcx.Memos where a.MemoId == Id select a).FirstOrDefault();

            Models.PettyCashComment addThisComment = new()
            {
               PettyCashId = memoToUpdate.MemoId,
                CreatedDate = DateTime.Now,

                Message = addCommentVM.NewComment,


                CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Name").Value,
                  UserId = usm.FindByNameAsync(User.Claims.FirstOrDefault(c => c.Type == "Name").Value).Result.Id,
            };

            dcx.PettyCashComments.Add(addThisComment);
            await dcx.SaveChangesAsync();

            return RedirectToAction("ViewMemos");
        }

        [HttpPost]
        public async Task<IActionResult> EditPettyCash()
        {
            return  RedirectToAction("ViewPettyCash");
        }

    }
}
