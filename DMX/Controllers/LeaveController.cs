using AspNetCoreHero.ToastNotification.Abstractions;
using DMX.Data;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DMX.Controllers
{
    public class LeaveController(XContext dContext, UserManager<AppUser>userManager, INotyfService notyfService) : Controller
    {

        public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser> usm=userManager;
        public readonly INotyfService notyf= notyfService;

        [HttpPost]
        public async Task<IActionResult> LeaveComment(string Id, MemoCommentVM addCommentVM)
        {

            Memo memoToUpdate = new();
            memoToUpdate = (from a in dcx.Memos where a.MemoId == Id select a).FirstOrDefault();

            Comment addThisComment = new()
            {
                TaskId = memoToUpdate.MemoId,
                CreatedDate = DateTime.Now,

                Message = addCommentVM.NewComment,


                CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Name").Value,
                //  UserId = usm.FindByNameAsync(User.Claims.FirstOrDefault(c => c.Type == "Name").Value).Result.Id,
            };

            dcx.LeaveComments.Add(addThisComment);
            await dcx.SaveChangesAsync();

            return RedirectToAction("ViewMemos");
        }

        [HttpPost]
        public async Task<IActionResult> MaternityLeaveComment(string Id, MemoCommentVM addCommentVM)
        {

            Memo memoToUpdate = new();
            memoToUpdate = (from a in dcx.Memos where a.MemoId == Id select a).FirstOrDefault();

            Comment addThisComment = new()
            {
                TaskId = memoToUpdate.MemoId,
                CreatedDate = DateTime.Now,

                Message = addCommentVM.NewComment,


                CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Name").Value,
                //  UserId = usm.FindByNameAsync(User.Claims.FirstOrDefault(c => c.Type == "Name").Value).Result.Id,
            };

            dcx.Comments.Add(addThisComment);
            await dcx.SaveChangesAsync();

            return RedirectToAction("ViewMemos");
        }
        public IActionResult ViewLeaves()
        {
            return ViewComponent("ViewLeaves");
        }
    }
}
