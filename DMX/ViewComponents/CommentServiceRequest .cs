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
    public class CommentServiceRequest(XContext dContext, UserManager<AppUser> userManager) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser> usm = userManager;
       
       
        public IViewComponentResult Invoke(string Id)
        {
            var decodedId=HttpUtility.UrlDecode( Id)?.Replace(" ", "+"); // sanitize
            var decryptedId = Encryption.Decrypt( decodedId);
            if (!Guid.TryParse(decryptedId, out Guid requestGuid)) ;
            ServiceRequest serviceToComment = new();
           serviceToComment = (from m in dcx.ServiceRequests.Include(m=>m.Category).Include(m => m.Priority).Include(m => m.RequestType).Include(m => m.Comments.OrderBy(m => m.CreatedDate)).ThenInclude(m => m.AppUser) where m.RequestId == requestGuid select m).FirstOrDefault();

            ServiceRequestCommentVM addCommentVM = new()
            {
                
                Title=serviceToComment.Title,
                Comments = serviceToComment.Comments.OrderBy(m => m.CreatedDate).ToList(),
            

            };
            

            return View(addCommentVM);
        }
    }
}

