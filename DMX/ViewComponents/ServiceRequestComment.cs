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
    public class ServiceComment:ViewComponent
    {
        public readonly XContext dcx;
        public readonly UserManager<AppUser> usm;
        public ServiceComment(XContext dContext, UserManager<AppUser> userManager)
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
            var decodedId=HttpUtility.UrlDecode(Id)?.Replace(" ","+");
            var decryptedId=Encryption.Decrypt(decodedId);
            if(!Guid.TryParse(decodedId, out Guid serviceGuid))
            {

            }

            ServiceRequest serviceRequestToEdit = new ServiceRequest();
            serviceRequestToEdit = (from m in dcx.ServiceRequests.Include(m => m.Comments.OrderBy(m => m.CreatedDate)) where m.RequestId == serviceGuid select m).FirstOrDefault();

            ServiceRequestCommentVM addCommentVM = new()
            {
                
                Comments = serviceRequestToEdit.Comments,
                Title = serviceRequestToEdit.RequestNumber,
                SelectedUsers = AssignedUsers,

        
                UsersList= new SelectList(usm.Users.ToList(), (nameof(AppUser.Id),nameof(AppUser.Fullname))),
            };
            

            return View(addCommentVM);
        }
    }
}
