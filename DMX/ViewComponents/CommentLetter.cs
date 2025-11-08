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
    public class CommentLetter(XContext dContext, UserManager<AppUser> userManager) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser> usm = userManager;
        
        public IViewComponentResult Invoke(string Id)
        {
            var decodedId = HttpUtility.UrlDecode(Id)?.Replace(" ", "+"); // sanitize
            var decryptedId = Encryption.Decrypt(decodedId);
            if(!Guid.TryParse(decryptedId, out Guid letterGuid))
            {
                return View("Error", "Invalid Letter Id format");
            }
            var letterToComment = (from d in dcx.Letters.Include(d=>d.LetterComments.OrderBy(l=>l.CreatedDate)) where d.LetterId ==letterGuid select d).FirstOrDefault();

            DocumentCommentVM addCommentVM = new()
            {
                MemoContent = letterToComment.AdditionalNotes,
                Comments = letterToComment.LetterComments.OrderBy(m => m.CreatedDate).ToList(),
                Title = letterToComment.ReferenceNumber,
                MemoId=letterToComment.LetterId,
                //SelectedUsers = AssignedUsers,
                Document=letterToComment.PDF,
                CommentCount = letterToComment.LetterComments.Count(),
                UsersList = new SelectList(usm.Users.ToList(), (nameof(AppUser.Id),nameof(AppUser.Fullname))),
            };
            

            return View(addCommentVM);
        }
    }
}
