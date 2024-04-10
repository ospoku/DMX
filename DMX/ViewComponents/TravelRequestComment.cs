using DMX.Data;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DMX.ViewComponents
{
    public class TravelRequestComment:ViewComponent
    {
        public readonly XContext dcx;
        public readonly UserManager<AppUser> usm;
        public TravelRequestComment(XContext dContext, UserManager<AppUser> userManager)
        {
            dcx = dContext;
            usm = userManager;
        }

        public IViewComponentResult Invoke(string Id)


        {
            var AssignedUsers= new List<string>();

            //var result = from u in dcx.Assignments where u.MemoId == Id select u.ApplicationUser.Id;
            //foreach (var user in result) {
            //    AssignedUsers.Add(user);
            //}

            TravelRequest travelRequestToEdit = new TravelRequest();
            travelRequestToEdit = (from m in dcx.TravelRequests.Include(m => m.Comments.OrderBy(m => m.CreatedDate)) where m.TravelRequestId == Id select m).FirstOrDefault();

            MemoCommentVM addCommentVM = new MemoCommentVM
            {
                MemoContent = travelRequestToEdit.PurposeofJourney,
               // Comments = travelRequestToEdit.Comments,
                Title = travelRequestToEdit.ReferenceNumber,
               // SelectedUsers = AssignedUsers,

        
                UsersList= new SelectList(usm.Users.ToList(), "Id", "UserName"),
            };
            

            return View(addCommentVM);
        }
    }
}
