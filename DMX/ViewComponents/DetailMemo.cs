using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using DMX.Data;
using DMX.Models;
using DMX.ViewModels;
using DMX.DataProtection;
using System.Web;

namespace DMX.ViewComponents
{
    public class DetailMemo(XContext dContext, UserManager<AppUser> userManager) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser> usm = userManager;

        public IViewComponentResult Invoke(string Id)
        {
            var decodedId=HttpUtility.UrlDecode(Id)?.Replace(" ","+");
            var decryptedId=Encryption.Decrypt(decodedId);
            if (!Guid.TryParse(decryptedId, out Guid memoGuid)) { }

            Memo memoDetail = new();
            memoDetail = (from m in dcx.Memos where m.PublicId == memoGuid & m.IsDeleted == false select m).FirstOrDefault();
            DetailMemoVM detailMemoVM = new()
            {
                Content = memoDetail.Content,
              
                Title = memoDetail.Title,
           

                
               SelectedUsers = (from x in dcx.MemoAssignments where x.PublicId == memoGuid select x.UserId).ToList(),
                UsersList = new SelectList(usm.Users.ToList(), (nameof(AppUser.Id),nameof(AppUser.Fullname))),
            };
            return View(detailMemoVM);
        }
    }
}
