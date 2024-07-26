using DMX.Data;
using DMX.DataProtection;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMX.ViewComponents
{
    public class CommentLetter(XContext dContext, UserManager<AppUser> userManager) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser> usm = userManager;

        public IViewComponentResult Invoke(string Id)


        {

            Letter? letterToComment = new Letter();

            letterToComment = (from d in dcx.Letters where d.LetterId == @Encryption.Decrypt(Id) select d).FirstOrDefault();

            DocumentCommentVM addCommentVM = new()
            {
                MemoContent = letterToComment.AdditionalNotes,
                Comments = (from c in dcx.LetterComments where c.LetterId == letterToComment.LetterId select c).ToList(),
                Title = letterToComment.ReferenceNumber,
                //SelectedUsers = AssignedUsers,
                Document=letterToComment.PDF,
        
                UsersList= new SelectList(usm.Users.ToList(), "Id", "UserName"),
            };
            

            return View(addCommentVM);
        }
    }
}
