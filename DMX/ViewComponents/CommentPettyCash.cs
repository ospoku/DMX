using DMX.Data;
using DMX.DataProtection;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DMX.ViewComponents
{
    public class CommentPettyCash(XContext dContext, UserManager<AppUser> userManager) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser> usm = userManager;

        public IViewComponentResult Invoke(string Id)


        {
            var AssignedUsers= new List<string>();

            //var result = from u in dcx.Assignments where u.MemoId == Id select u.ApplicationUser.Id;
            //foreach (var user in result) {
            //    AssignedUsers.Add(user);
            //}

            PettyCash pettycashToComment = new();
            pettycashToComment = (from p in dcx.PettyCash.Include(p => p.Comments.OrderBy(m => m.CreatedDate)).ThenInclude(c => c.AppUser) where p.PettyCashId.ToString() == @Encryption.Decrypt(Id) select p).FirstOrDefault();

            PettyCashCommentVM  addCommentVM = new()
            {
               Amount = pettycashToComment.Amount,
              
                Purpose = pettycashToComment.Purpose,
                SelectedUsers = AssignedUsers,
                Comments = pettycashToComment.Comments.OrderBy(m => m.CreatedDate).ToList(),
                CommentCount = pettycashToComment.Comments.Count(),
                UsersList = new SelectList(usm.Users.ToList(), (nameof(AppUser.Id),nameof(AppUser.Fullname))),
            };
            

            return View(addCommentVM);
        }
    }
}
