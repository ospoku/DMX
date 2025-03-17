using CsvHelper.Configuration.Attributes;
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
    public class CommentTravelRequest(XContext dContext, UserManager<AppUser> userManager) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser> usm = userManager;
       
       
        public IViewComponentResult Invoke(string Id)
        {
            TravelRequest travelToComment = new();
            travelToComment = (from m in dcx.TravelRequests.Include(m=>m.TravelType).Include(m => m.Comments.OrderBy(m => m.CreatedDate)).ThenInclude(m => m.AppUser) where m.TravelRequestId == @Encryption.Decrypt(Id) select m).FirstOrDefault();

            TravelRequestCommentVM addCommentVM = new()
            {
                StartDate = travelToComment.StartDate,

                Comments = travelToComment.Comments.OrderBy(m => m.CreatedDate).ToList(),
                EndDate = travelToComment.EndDate,
                CommentCount = travelToComment.Comments.Count(),
                Purpose=travelToComment.Purpose,
                TravelTypeId=travelToComment.TravelType.Name,

            };
            

            return View(addCommentVM);
        }
    }
}

