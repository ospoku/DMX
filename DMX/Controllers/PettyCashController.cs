using AspNetCoreHero.ToastNotification.Abstractions;
using DMX.Data;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DMX.Controllers
{
    public class PettyCashController(XContext dContext, UserManager<AppUser> userManager, INotyfService notyfService
          ) :Controller
    {

        public readonly UserManager<AppUser> usm = userManager;
        public readonly XContext dcx = dContext;
        private readonly INotyfService notyf = notyfService;



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
            PettyCash addThisPettyCash = new()
            {
              
                Date = addPettyCashVM.Date,
                Amount = addPettyCashVM.Amount,
                Purpose = addPettyCashVM.Purpose,
                Description = addPettyCashVM.Description,
                CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Name").Value,
                CreatedDate = DateTime.UtcNow,


            };
            dcx.PettyCashes.Add(addThisPettyCash);
            foreach (var user in addPettyCashVM.SelectedUsers)
            {


                dcx.PettyCashAssignments.Add(new PettyCashAssignment
                {
                    PettyCashId = addThisPettyCash.PettyCashId,
                    AppUserId = user,
                    CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Name").Value,
                    CreatedDate = DateTime.UtcNow,
                });
            }
            if (await dcx.SaveChangesAsync(User.Claims.FirstOrDefault(c => c.Type == "Name").Value) > 0)
            {
                notyf.Success("Petty Cash successfully saved", 5);

                return RedirectToAction("ViewPettyCash");
            }
            else
            {
                notyf.Error("Error, Petty Cash could not be saved!!!", 5);
            }

            return ViewComponent("ViewPettyCash");
        }


        [HttpGet]
        public IActionResult AddPettyCash() => ViewComponent("AddPettyCash");
        [HttpPost]
        public async Task<IActionResult> PettyCashComment(string Id, MemoCommentVM addCommentVM)
        {

            Memo memoToUpdate = new();
            memoToUpdate = (from a in dcx.Memos where a.MemoId == Id select a).FirstOrDefault();

            PettyCashComment addThisComment = new()
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

    }
}
