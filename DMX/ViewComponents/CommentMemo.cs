using CsvHelper.Configuration.Attributes;
using DMX.Data;
using DMX.DataProtection;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Web;

namespace DMX.ViewComponents
{
    public class CommentMemo(XContext dContext, UserManager<AppUser> userManager) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser> usm = userManager;
       
       
        public IViewComponentResult Invoke(string Id)
        {
            var decodedId =HttpUtility.UrlDecode( Id)?.Replace(" ", "+"); // sanitize
            var decryptedId = Encryption.Decrypt( decodedId);
            if (!Guid.TryParse(decryptedId, out Guid memoGuid));

                Memo memoToComment = new();
            memoToComment = (from m in dcx.Memos.Include(m => m.MemoComments.OrderBy(m => m.CreatedDate)).ThenInclude(c => c.AppUser) where m.MemoId ==  memoGuid select m).FirstOrDefault();

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

