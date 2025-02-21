﻿using DMX.Data;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DMX.ViewComponents
{
    public class EditServiceRequest:ViewComponent
    {
        public readonly XContext dcx;
        public EditServiceRequest(XContext dContext)
        {
            dcx = dContext;
        }

        public IViewComponentResult Invoke(string MemoId)


        {
           

            ServiceRequest serviceRequestToEdit = new ServiceRequest();
            serviceRequestToEdit = (from sr in dcx.ServiceRequests.Include(sr => sr.Comments.OrderBy(m=>m.CreatedDate)) where sr.RequestId ==sr.RequestId select sr ).FirstOrDefault();

            EditServiceRequestVM editServiceRequestVM = new EditServiceRequestVM
            {
           


            };
            

            return View(editServiceRequestVM);
        }
    }
}
