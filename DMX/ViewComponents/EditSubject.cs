using DMX.Data;
using DMX.DataProtection;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DMX.ViewComponents
{
    public class EditSubject:ViewComponent
    {
        public readonly XContext dcx;
        public EditSubject(XContext dContext)
        {
            dcx = dContext;
        }

        public IViewComponentResult Invoke(string Id)


        {
           

            ServiceRequest serviceRequestToEdit = new ServiceRequest();
            serviceRequestToEdit = (from sr in dcx.ServiceRequests.Include(sr => sr.Comments.OrderBy(m=>m.CreatedDate)) where sr.RequestId ==Encryption.Decrypt(Id) select sr ).FirstOrDefault();

            EditSubjectVM editServiceRequestVM = new EditSubjectVM
            {
           


            };
            

            return View(editServiceRequestVM);
        }
    }
}
