using DMX.Data;
using DMX.DataProtection;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DMX.ViewComponents
{
    public class EditTravelRequest(XContext dContext) : ViewComponent
    {
        public readonly XContext dcx = dContext;

        public IViewComponentResult Invoke(string TravelRequestId)


        {
           

            TravelRequest travelRequestToEdit = new TravelRequest();
            travelRequestToEdit = (from tr in dcx.TravelRequests.Include(m => m.Comments.OrderBy(m=>m.CreatedDate)) where tr.TravelRequestId==@Encryption.Decrypt(TravelRequestId )select tr ).FirstOrDefault();

            EditTravelRequestVM editTravelRequestVM = new EditTravelRequestVM
            {
               Purpose=travelRequestToEdit.Purpose, 
               StartDate=travelRequestToEdit.StartDate,
               EndDate=travelRequestToEdit.EndDate,
            };
            

            return View(editTravelRequestVM);
        }
    }
}
