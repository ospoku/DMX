using AspNetCoreHero.ToastNotification.Abstractions;
using DMX.Data;
using DMX.Models;
using DMX.Services;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DMX.Controllers
{
    public class TravelRequestController(XContext dContext,UserManager<AppUser>userManager,INotyfService notyfService,EntityService entityService) : Controller
    {
     public readonly UserManager<AppUser>usm= userManager;
public readonly XContext dcx = dContext;
        public readonly INotyfService notyf = notyfService;
        public readonly EntityService entityServ = entityService;
            
        [HttpPost]
        public async Task<IActionResult> AddTravelRequest(AddTravelRequestVM addTravelRequestVM)
        {
            if (addTravelRequestVM.SelectedUsers?.Any() != true)
            {
                notyf.Error("You must select at least one user for assignment.", 5);
                return RedirectToAction("ViewTravelRequests");
            }

            try
            {
                var existingRequest = await dcx.TravelRequests.FirstOrDefaultAsync(p =>
    p.TravelTypeId.ToLower() == addTravelRequestVM.TravelTypeId.ToLower() &&
   p.Purpose.ToLower() == addTravelRequestVM.Purpose.ToLower());

                if (existingRequest != null)
                {
                    notyf.Error("This record already exists.");
                    return RedirectToAction("ViewTravelRequests");
                }

                // Create and populate the patient object
                TravelRequest addThisTravelRequest = new()
                {
                    EndDate = addTravelRequestVM.EndDate,
                    StartDate = addTravelRequestVM.StartDate,

                    DateofReturn = addTravelRequestVM.DateofReturn,
                    ConferenceFee = addTravelRequestVM.ConferenceFee,
                    FuelClaim= addTravelRequestVM.FuelClaim,
                    OtherExpenses = addTravelRequestVM.OtherExpenses,
                    TravelTypeId = addTravelRequestVM.TravelTypeId,
                    Purpose = addTravelRequestVM.Purpose,

                };
                 

                // Add patient to the database
                var requestAdded = await entityServ.AddEntityAsync(addThisTravelRequest, User);
                if (!requestAdded)
                {
                    notyf.Error("Failed to create request.", 5);
                    return RedirectToAction("Error", "Home", new { message = "Patient creation failed." });
                }

                // Assign users if any are selected
                if (addTravelRequestVM.SelectedUsers?.Any() == true)
                {
                    foreach (var userId in addTravelRequestVM.SelectedUsers)
                    {
                        var assignment = new TravelRequestAssignment
                        {
                            TravelRequestId = addThisTravelRequest.TravelRequestId,
                            AppUserId = userId,
                        };

                        var assignmentAdded = await entityServ.AddEntityAsync(assignment, User);
                        if (!assignmentAdded)
                        {
                            notyf.Error("Failed to assign user.", 5);
                            return RedirectToAction("Error", "Home", new { message = "User assignment failed." });
                        }
                    }
                    notyf.Success("Request and assignments successfully processed.", 5);
                }
                else
                {
                    notyf.Success("Request created successfully, no users assigned.", 5);
                }

                return RedirectToAction("ViewTravelRequests");
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.Error.WriteLine(ex);
                notyf.Error("An unexpected error occurred.", 5);
                return RedirectToAction("Error", "Home", new { message = "An unexpected error occurred." });
            }
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
