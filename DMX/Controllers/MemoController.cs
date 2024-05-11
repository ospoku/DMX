using AspNetCoreHero.ToastNotification.Abstractions;
using DMX.Data;
using DMX.DataProtection;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DMX.Controllers
{
    public class MemoController(XContext dContext, UserManager<AppUser>userManager, INotyfService notyfService) : Controller
    {
        public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser>usm=userManager;
        public readonly INotyfService notyf =notyfService;
        [HttpPost]
        public async Task<IActionResult> EditMemo(string Id, EditMemoVM editMemoVM)
        {

            Memo updateThisMemo = (from a in dcx.Memos where a.MemoId == @Encryption.Decrypt(Id) select a).FirstOrDefault();


            updateThisMemo.Content = editMemoVM.Content;
            updateThisMemo.ModifiedDate = DateTime.UtcNow;

            updateThisMemo.Title = editMemoVM.Title;


            updateThisMemo.ModifiedBy = User.Claims.FirstOrDefault(c => c.Type == "Name").Value;
            //updateThisMemo.UserId = usm.FindByNameAsync(User.Claims.FirstOrDefault(c => c.Type == "Name").Value).Result.Id;

            //Assignment updateThisAssignment = (from x in dcx.MemoAssignments where x.TaskId == @Encryption.Decrypt(Id) select x).FirstOrDefault();

           // updateThisAssignment.SelectedUsers = string.Join(',', editMemoVM.SelectedUsers);
            dcx.Memos.Attach(updateThisMemo);

            dcx.Entry(updateThisMemo).State = EntityState.Modified;
            if (await dcx.SaveChangesAsync() > 0)
            {
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
        //[HttpPost]
        //public IActionResult PrintMemo(string Id, MemoCommentVM printVM)
        //{

        //    Memo memoToPrint = (from m in dcx.Memos.Include(m => m.MemoComments.OrderBy(m => m.CreatedDate)) where m.MemoId == @Encryption.Decrypt(Id) select m).FirstOrDefault();


        //    printVM = new MemoCommentVM
        //    {
        //        Comments = memoToPrint.MemoComments.ToList(),
        //        Title = memoToPrint.Title,
        //        Sender=memoToPrint.Sender,
        //        Recipient=memoToPrint.Recipient,
        //       CreatedDate=memoToPrint.CreatedDate.Value,
        //    MemoContent = memoToPrint.Content,
        //        SelectedUsers = [.. dcx.MemoAssignments.Where(x => x.MemoId == @Encryption.Decrypt(Id)).Select(p => p.AppUserId)],
        //    };

        //    return View(printVM);
        //}
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
            string RefN = "M" + rand.Next((int)Math.Pow(10,digit-1),(int)Math.Pow(10,digit));

            Memo addThisMemo = new()
            {
                Content = addMemoVM.Content,
                Recipient = addMemoVM.Receipient,
                ReferenceId = RefN,
                Title =addMemoVM.Title,
                CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Name").Value,
                CreatedDate = DateTime.UtcNow,
            };
            dcx.Memos.Add(addThisMemo);
            foreach (var user in addMemoVM.SelectedUsers)
            {

                dcx.MemoAssignments.Add(new MemoAssignment
                {
                    MemoId = addThisMemo.MemoId,
                    AppUserId = user,
                    CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Name").Value,
                    CreatedDate = DateTime.UtcNow,
                });
            }
            if (await dcx.SaveChangesAsync(User.Claims.FirstOrDefault(c => c.Type == "Name").Value) > 0)
            {
                notyf.Success("Memo successfully saved", 5);

                return RedirectToAction("ViewMemos");
            }
            else
            {
                notyf.Error("Error, Memo could not be saved!!!", 5);
                return RedirectToAction("ViewMemos");
            }

          
        }
        [HttpGet]
        public IActionResult EditMemo(string Id)
        => ViewComponent("EditMemo", Id);
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
                CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Name").Value,
                  UserId = usm.FindByNameAsync(User.Claims.FirstOrDefault(c => c.Type == "Name").Value).Result.Id,
            };

            dcx.MemoComments.Add(addThisComment);
            await dcx.SaveChangesAsync();

            return RedirectToAction("ViewMemos");
        }

        [HttpPost]
        public IActionResult DeleteMemo(string Id)
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
