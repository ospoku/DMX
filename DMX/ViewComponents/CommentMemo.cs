using DMX.ViewModels;
using DMXDomain.Models;
using DMXInfrastructure.Data;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DMX.ViewComponents
{
    public class CommentMemo(XContext dContext, UserManager<AppUser> userManager, IDataProtectionProvider provider) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser> usm = userManager;
        public readonly IDataProtector protector = provider.CreateProtector("IdProtector");

        public IViewComponentResult Invoke(string Id)
        {
           
            var decryptedId = protector.Unprotect(Id);
            if (!Guid.TryParse(decryptedId, out Guid memoGuid))
            {

                return View("Error", "Invalid Excuse Duty Id format");
            }
                Memo memoToComment = new();
                memoToComment = (from m in dcx.Memos.Include(m => m.MemoComments.OrderBy(m => m.CreatedDate)).ThenInclude(c => c.AppUser) where m.PublicId == memoGuid select m).FirstOrDefault();

                MemoCommentVM addCommentVM = new()
                {
                    MemoContent = memoToComment.Content,

                    Comments = memoToComment.MemoComments.OrderBy(m => m.CreatedDate).ToList(),
                    Title = memoToComment.Title,
                    CommentCount = memoToComment.MemoComments.Count(),

                };


                return View(addCommentVM);
            }
        }
    }


