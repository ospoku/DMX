using DMX.Data;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMX.ViewComponents
{
    public class EditMemo:ViewComponent
    {
        public readonly XContext dcx;
        public readonly UserManager<AppUser> usm;
        public EditMemo(XContext dContext,UserManager<AppUser> userManager)
        {
            dcx = dContext;
            usm = userManager;
        }

        public IViewComponentResult Invoke(string Id)
        {

            var stringIDs = (from x in dcx.Assignments where x.TaskId == Id select x.SelectedUsers).FirstOrDefault().Split(',');
        
            var memoToEdit = (from m in dcx.Memos where m.MemoId == Id select m).FirstOrDefault();

            EditMemoVM editMemoVM = new EditMemoVM
            {

                Title = memoToEdit.Title,
                Content = memoToEdit.Content,
                SelectedUsers = stringIDs,
                UsersList=  new SelectList(usm.Users.ToList(), "Id", "UserName"),
            };


            return View(editMemoVM);
        }
    }
}
