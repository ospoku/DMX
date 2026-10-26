using DMX.ViewModels;
using DMXDomain.Models;
using DMXInfrastructure.Data;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DMX.ViewComponents
{
    public class CommentServiceRequest(XContext dContext, UserManager<AppUser> userManager, IDataProtectionProvider provider) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser> usm = userManager;
        public readonly IDataProtector protector = provider.CreateProtector("IdProtector");
       
        public IViewComponentResult Invoke(string Id)
        {
            
            var decryptedId = protector.Unprotect(Id);
            if (!Guid.TryParse(decryptedId, out Guid requestGuid)) ;
            ServiceRequest serviceToComment = new();
           serviceToComment = (from m in dcx.ServiceRequests.Include(m=>m.Category).Include(m => m.Priority).Include(m => m.RequestType).Include(m => m.Comments.OrderBy(m => m.CreatedDate)).ThenInclude(m => m.AppUser) where m.PublicId == requestGuid select m).FirstOrDefault();

            ServiceRequestCommentVM addCommentVM = new()
            {
                
                Title=serviceToComment.Title,
                Comments = serviceToComment.Comments.OrderBy(m => m.CreatedDate).ToList(),
            

            };
            

            return View(addCommentVM);
        }
    }
}

