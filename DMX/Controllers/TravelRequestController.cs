using AspNetCoreHero.ToastNotification.Abstractions;
using DMX.Data;
using DMX.Models;
using DMX.Services;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DMX.Controllers
{
    public class TravelRequestController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly XContext _context;
        private readonly INotyfService _notyfService;
        private readonly EntityService _entityService;

        public TravelRequestController(
            XContext context,
            UserManager<AppUser> userManager,
            INotyfService notyfService,
            EntityService entityService)
        {
            _userManager = userManager;
            _context = context;
            _notyfService = notyfService;
            _entityService = entityService;
        }

        [HttpGet]
        public IActionResult ViewTravelRequests() => ViewComponent("ViewTravelRequests");

        [HttpGet]
        public IActionResult AddTravelRequest() => ViewComponent("AddTravelRequest");

        [HttpPost]
        public async Task<IActionResult> AddTravelRequest(AddTravelRequestVM addTravelRequestVm)
        {
            if (addTravelRequestVm.SelectedUsers?.Any() != true)
            {
                _notyfService.Error("You must select at least one user for assignment.", 5);
                return RedirectToAction("ViewTravelRequests");
            }

            try
            {
                // Check for duplicate travel requests
                var existingRequest = await _context.TravelRequests
                    .FirstOrDefaultAsync(p =>
                        p.TravelTypeId.ToLower() == addTravelRequestVm.TravelTypeId.ToLower() &&
                        p.Purpose.ToLower() == addTravelRequestVm.Purpose.ToLower());

                if (existingRequest != null)
                {
                    _notyfService.Error("This record already exists.");
                    return RedirectToAction("ViewTravelRequests");
                }

                // Create new travel request
                var newTravelRequest = new TravelRequest
                {
                    EndDate = addTravelRequestVm.EndDate,
                    StartDate = addTravelRequestVm.StartDate,
                    DateofReturn = addTravelRequestVm.DateofReturn,
                    ConferenceFee = addTravelRequestVm.ConferenceFee,
                    FuelClaim = addTravelRequestVm.FuelClaim,
                    OtherExpenses = addTravelRequestVm.OtherExpenses,
                    TravelTypeId = addTravelRequestVm.TravelTypeId,
                    Purpose = addTravelRequestVm.Purpose
                };

                // Add travel request to the database
                bool requestAdded = await _entityService.AddEntityAsync(newTravelRequest, User);
                if (!requestAdded)
                {
                    _notyfService.Error("Failed to create travel request.", 5);
                    return RedirectToAction("Error", "Home", new { message = "Travel request creation failed." });
                }

                // Assign selected users to the travel request
                foreach (var userId in addTravelRequestVm.SelectedUsers)
                {
                    var assignment = new TravelRequestAssignment
                    {
                        TravelRequestId = newTravelRequest.TravelRequestId,
                        AppUserId = userId
                    };

                    bool assignmentAdded = await _entityService.AddEntityAsync(assignment, User);
                    if (!assignmentAdded)
                    {
                        _notyfService.Error($"Failed to assign user {userId}.", 5);
                    }
                }

                _notyfService.Success("Travel request and assignments successfully processed.", 5);
                return RedirectToAction("ViewTravelRequests");
            }
            catch (Exception ex)
            {
                _notyfService.Error("An unexpected error occurred.", 5);
                Console.Error.WriteLine(ex);
                return RedirectToAction("Error", "Home", new { message = "An unexpected error occurred." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> TravelRequestComment(string id, MemoCommentVM commentVm)
        {
            try
            {
                // Find the travel request by ID
                var travelRequest = await _context.TravelRequests
                    .FirstOrDefaultAsync(t => t.TravelRequestId == id);

                if (travelRequest == null)
                {
                    return NotFound();
                }

                // Create a new comment
                var newComment = new TravelRequestComment
                {
                    TravelRequestId = travelRequest.TravelRequestId,
                    Message = commentVm.NewComment,
                    CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Name")?.Value,
                    CreatedDate = DateTime.Now
                };

                // Add the comment to the database
                _context.TravelRequestComments.Add(newComment);
                await _context.SaveChangesAsync();

                _notyfService.Success("Comment successfully added.", 5);
                return RedirectToAction("ViewTravelRequests");
            }
            catch (Exception ex)
            {
                _notyfService.Error("An error occurred while adding the comment.", 5);
                return RedirectToAction("Error", "Home", new { message = "An error occurred while processing the comment." });
            }
        }
    }
}