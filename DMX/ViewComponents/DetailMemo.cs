using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using DMX.Data;
using DMX.Models;
using DMX.ViewModels;

namespace DMX.ViewComponents
{
    public class DetailMemo(XContext dContext, UserManager<AppUser> userManager) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser> usm = userManager;

        public IViewComponentResult Invoke(string Id)
        {
             var stringIDs = (from x in dcx.Assignments where x.TaskId == Id select x.SelectedUsers).FirstOrDefault().Split(',');

            Memo memoDetail = new();
           memoDetail = (from m in dcx.Memos where m.MemoId == Id & m.IsDeleted == false select m).FirstOrDefault();
            DetailMemoVM detailMemoVM = new()
            {



              
                SelectedUsers = (from x in dcx.Assignments where x.TaskId == Id select x.SelectedUsers).FirstOrDefault().Split(','),
                UsersList = new SelectList(usm.Users.ToList(), "Id", "UserName"),



            };
            return View(detailMemoVM);
        }
    }
}
