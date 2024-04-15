using DMX.Data;
using DMX.DataProtection;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMX.ViewComponents
{
    public class EditMemo(XContext dContext, UserManager<AppUser> userManager) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser> usm = userManager;

        public IViewComponentResult Invoke(string Id)
        {

            //var stringIDs = (from x in dcx.Assignments where x.TaskId == Id select x.SelectedUsers).FirstOrDefault().Split(',');
        
            var memoToEdit = (from m in dcx.Memos where m.MemoId == @Encryption.Decrypt(Id) select m).FirstOrDefault();

            EditMemoVM editMemoVM = new()
            {

                Title = memoToEdit.Title,
                Content = memoToEdit.Content,
                SelectedUsers = (from x in dcx.MemoAssignments where x.MemoId == @Encryption.Decrypt(Id) select x.AppUserId).ToList(),
                UsersList = new SelectList(usm.Users.ToList(), "Id", "UserName"),
            };


            return View(editMemoVM);
        }
    }
}
