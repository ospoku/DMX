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
    public class LeaveController(XContext dContext, UserManager<AppUser>userManager,EntityService entityService, INotyfService notyfService) : Controller
    {

        public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser> usm=userManager;
        public readonly INotyfService notyf= notyfService;
        public readonly EntityService entityServ = entityService;

        [HttpPost]
        public async Task<IActionResult> LeaveComment(string Id, MemoCommentVM addCommentVM)
        {
            try
            {
                Leave leaveToComment = dcx.Leaves.Where(l => l.LeaveId == @Encryption.Decrypt(Id)).Select(l => l).FirstOrDefault();

                LeaveComment addThisComment = new()
                {
                    LeaveId = leaveToComment.LeaveId,
                    CreatedDate = DateTime.Now,

                    Message = addCommentVM.NewComment,
                };

                bool result = await entityServ.AddEntityAsync(addThisComment, User);

                if (result)
                {

                    notyf.Success("Record successfully saved.", 5);
                    return RedirectToAction("ViewLeaves");
                }
                else
                {
                    notyf.Error("Record could not be saved.", 5);
                    return RedirectToAction("ViewLeaves");
                }
               
            }
            catch
            {
                return RedirectToAction("Error","Home", new { message = "An error occurred while processing the request." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> MaternityLeaveComment(string Id, MemoCommentVM addCommentVM)
        {
            try
            {

                Leave leaveToComment = new();
                leaveToComment = (from a in dcx.Leaves where a.LeaveId == Encryption.Decrypt(Id) select a).FirstOrDefault();

                LeaveComment addThisComment = new()
                {
                    LeaveId = leaveToComment.LeaveId,
                    CreatedDate = DateTime.Now,
                    Message = addCommentVM.NewComment

                };
                bool result = await entityServ.AddEntityAsync(addThisComment, User); 
                if (result)
                {
                    notyf.Success("Record successfully saved!", 5);
                    return RedirectToAction("ViewLeaves");
                }
                else
                {
                    notyf.Error("Record could not be saved.", 5);
                    return RedirectToAction("ViewLeaves");
                }
            }
            catch
            {
                return RedirectToAction("ErrorPage", new { message = "An error occurred while processing the request." });
            }
        }
        public IActionResult ViewLeaves()
        {
            return ViewComponent("ViewLeaves");
        }
    }
}
