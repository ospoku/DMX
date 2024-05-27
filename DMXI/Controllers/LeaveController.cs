using AspNetCoreHero.ToastNotification.Abstractions;
using DMX.Data;
using DMX.DataProtection;
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

            Leave leaveToComment =  dcx.Leaves.Where(l=>l.LeaveId==@Encryption.Decrypt(Id)).Select(l=>l).FirstOrDefault();

            LeaveComment addThisComment = new()
            {
                LeaveId = leaveToComment.LeaveId,
                CreatedDate = DateTime.Now,

                Message = addCommentVM.NewComment,


                CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Name").Value,
               UserId = usm.FindByNameAsync(User.Claims.FirstOrDefault(c => c.Type == "Name").Value).Result.Id,
            };

            dcx.LeaveComments.Add(addThisComment);
            await dcx.SaveChangesAsync();

            return RedirectToAction("ViewMemos");
        }

        [HttpPost]
        public async Task<IActionResult> MaternityLeaveComment(string Id, MemoCommentVM addCommentVM)
        {

            Leave leaveToComment = new();
            leaveToComment = (from a in dcx.Leaves where a.LeaveId ==Encryption.Decrypt( Id) select a).FirstOrDefault();

            LeaveComment addThisComment = new()
            {
                LeaveId = leaveToComment.LeaveId,
                CreatedDate = DateTime.Now,

                Message = addCommentVM.NewComment,


                CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Name").Value,
                 UserId = usm.FindByNameAsync(User.Claims.FirstOrDefault(c => c.Type == "Name").Value).Result.Id,
            };

            dcx.LeaveComments.Add(addThisComment);
            await dcx.SaveChangesAsync();

            return RedirectToAction("ViewMemos");
        }
        public IActionResult ViewLeaves()
        {
            return ViewComponent("ViewLeaves");
        }
    }
}
