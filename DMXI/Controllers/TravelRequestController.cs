﻿using AspNetCoreHero.ToastNotification.Abstractions;
using DMX.Data;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DMX.Controllers
{
    public class TravelRequestController(XContext dContext,UserManager<AppUser>userManager,INotyfService notyfService) : Controller
    {
     public readonly UserManager<AppUser>usm= userManager;
public readonly XContext dcx = dContext;
        public readonly INotyfService notyf = notyfService;

        [HttpPost]
        public async Task<IActionResult> AddTravelRequest(AddTravelRequestVM addTravelRequestVM)
        {
            
                TravelRequest addThisTravelRequest = new()
                {

                    ReferenceNumber = addTravelRequestVM.ReferenceNumber,
                 
                    ConferenceFee = addTravelRequestVM.ConferenceFee,
                    DepartureDate = addTravelRequestVM.DepartureDate,
                    TransportExpenses = addTravelRequestVM.TransportExpenses,
                   
                    DateofReturn = addTravelRequestVM.DateofReturn,

                
                    FuelClaim = addTravelRequestVM.FuelClaim,
                    AmountDue = addTravelRequestVM.AmountDue,
                    Purpose = addTravelRequestVM.PurposeofJourney,


                    IsDeleted = false,
                    CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Name").Value,
                    CreatedDate = DateTime.Now,
                };
                dcx.TravelRequests.Add(addThisTravelRequest);
                foreach (var user in addTravelRequestVM.SelectedUsers)
                {


                    dcx.TravelRequestAssignments.Add(new TravelRequestAssignment
                    {
                        TravelRequestId = addThisTravelRequest.TravelRequestId,
                        AppUserId = user,
                        CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Name").Value,
                        CreatedDate = DateTime.Now,

                    });
                }

                if (await dcx.SaveChangesAsync(userId: User?.FindFirst(c => c.Type == "Name").Value) > 0)
                {
                    notyf.Success("Client successfully created.");
                    return RedirectToAction("ViewTravelRequests");

                }
                else
                {
                    notyf.Error("Member creation error!!! Please try again");
                }
                return RedirectToAction("AddTravelREquest");

           
     
              


        }
        [HttpPost]
        public async Task<IActionResult> TravelRequestComment(string Id, MemoCommentVM addCommentVM)
        {

            Memo memoToUpdate = new();
            memoToUpdate = (from a in dcx.Memos where a.MemoId == Id select a).FirstOrDefault();

            TravelRequestComment addThisComment = new()
            {
                TravelRequestId = memoToUpdate.MemoId,
                CreatedDate = DateTime.Now,

                Message = addCommentVM.NewComment,


                CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Name").Value,
                //  UserId = usm.FindByNameAsync(User.Claims.FirstOrDefault(c => c.Type == "Name").Value).Result.Id,
            };

            dcx.TravelRequestComments.Add(addThisComment);
            await dcx.SaveChangesAsync();

            return RedirectToAction("ViewMemos");
        }
        [HttpGet]
        public IActionResult AddTravelRequest() => ViewComponent("AddTravelRequest");
        [HttpGet]
        public IActionResult ViewTravelRequests()
        {
            return ViewComponent("ViewTravelRequests");
        }
    }
}