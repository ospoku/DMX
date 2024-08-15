using AspNetCoreHero.ToastNotification.Abstractions;
using DMX.Data;
using DMX.DataProtection;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DMX.Controllers
{
   
    public class MemoController(XContext dContext, UserManager<AppUser> userManager, INotyfService notyfService , IAuthorizationService authorizationService) : Controller
    {
        public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser> usm = userManager;
        public readonly INotyfService notyf = notyfService;
        public readonly IAuthorizationService auth=authorizationService;


        [HttpPost]
        public async Task<IActionResult> EditMemo(string Id, EditMemoVM editMemoVM)
        {

            Memo updateThisMemo = (from a in dcx.Memos where a.MemoId == @Encryption.Decrypt(Id) select a).FirstOrDefault();


            updateThisMemo.Content = editMemoVM.Content;
            updateThisMemo.ModifiedDate = DateTime.UtcNow;

            updateThisMemo.Title = editMemoVM.Title;


            updateThisMemo.ModifiedBy = usm.GetUserAsync(User).Result.UserName;

            dcx.Memos.Attach(updateThisMemo);

            dcx.Entry(updateThisMemo).State = EntityState.Modified;
            foreach (var user in editMemoVM.SelectedUsers)
            {

                dcx.MemoAssignments.Add(new MemoAssignment
                {
                    MemoId = editMemoVM.MemoId,
                    AppUserId = user,
                    CreatedBy = usm.GetUserAsync(User).Result.UserName,
                    CreatedDate = DateTime.UtcNow,
                });
            }

            if (await dcx.SaveChangesAsync(usm.GetUserAsync(User).Result.UserName) > 0)
            {
                notyf.Success("Record successfully saved", 5);

                return RedirectToAction("ViewMemos");
            }
            else
            {
                return ViewComponent("EditMemo");
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
            return ViewComponent("ViewMemos");
        }
        [HttpGet]
        public IActionResult AddMemo()
        {
            return ViewComponent("AddMemo");
        }
        [HttpPost]
        public async Task<IActionResult> AddMemo(AddMemoVM addMemoVM)
        { var rand = new Random();
            int digit = 5;
            string RefN = "M" + rand.Next((int)Math.Pow(10, digit - 1), (int)Math.Pow(10, digit));

            Memo addThisMemo = new()
            {
                Content = addMemoVM.Content,

                ReferenceId = RefN,
                Title = addMemoVM.Title,
                CreatedBy = usm.GetUserAsync(User).Result.UserName,
                CreatedDate = DateTime.UtcNow,
            };
            dcx.Memos.Add(addThisMemo);
            foreach (var user in addMemoVM.SelectedUsers)
            {

                dcx.MemoAssignments.Add(new MemoAssignment
                {
                    MemoId = addThisMemo.MemoId,
                    AppUserId = user,
                    CreatedBy = usm.GetUserAsync(User).Result.UserName,
                    CreatedDate = DateTime.UtcNow,
                });
            }
            if (await dcx.SaveChangesAsync(usm.GetUserAsync(User).Result.UserName) > 0)
            {
                notyf.Success("Record successfully saved", 5);

                return RedirectToAction("ViewMemos");
            }
            else
            {
                notyf.Error("Error, Record could not be saved!!!", 5);
                return RedirectToAction("ViewMemos");
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
