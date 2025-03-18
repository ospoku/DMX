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
    public class CommentExcuseDuty(XContext dContext, UserManager<AppUser> userManager) : ViewComponent
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

            ExcuseDuty dutyToComment = new();
            dutyToComment = (from m in dcx.ExcuseDuties.Include(m => m.ExcuseDutyComments.OrderBy(m => m.CreatedDate)).ThenInclude(u =>u.AppUser) where m.Id == @Encryption.Decrypt(Id) select m).FirstOrDefault();

            ExcuseDutyCommentVM addCommentVM = new()
            {
               Diagnosis  = dutyToComment.Diagnosis,
                PatientName=dutyToComment.PatientName,
               PatientId  = dutyToComment.PatientId,
               Days=dutyToComment.ExcuseDays,
               DischargeDate=dutyToComment.DateofDischarge,
                SelectedUsers = AssignedUsers,
                CommentCount = dutyToComment.ExcuseDutyComments.Count(),
                Comments = dutyToComment.ExcuseDutyComments.OrderBy(m => m.CreatedDate).ToList(),

                UsersList = new SelectList(usm.Users.ToList(), (nameof(AppUser.Id),nameof(AppUser.Fullname))),
            };
            

            return View(addCommentVM);
        }
    }
}
