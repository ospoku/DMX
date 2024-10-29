using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using DMX.Data;
using DMX.Models;
using DMX.ViewModels;
using DMX.DataProtection;

namespace DMX.ViewComponents
{
    public class DetailMemo(XContext dContext, UserManager<AppUser> userManager) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser> usm = userManager;

        public IViewComponentResult Invoke(string Id)
        {


            Memo memoDetail = new();
            memoDetail = (from m in dcx.Memos where m.MemoId == @Encryption.Decrypt(Id) & m.IsDeleted == false select m).FirstOrDefault();
            DetailMemoVM detailMemoVM = new()
            {
                Content = memoDetail.Content,
              
                Title = memoDetail.Title,
           

                
               SelectedUsers = (from x in dcx.MemoAssignments where x.MemoId == @Encryption.Decrypt(Id) select x.UserId).ToList(),
                UsersList = new SelectList(usm.Users.ToList(), "Id", "UserName"),
            };
            return View(detailMemoVM);
        }
    }
}
