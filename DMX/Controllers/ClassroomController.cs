using AspNetCoreHero.ToastNotification.Abstractions;
using DMX.Data;
using DMX.DataProtection;
using DMX.Models;
using DMX.Services;
using DMX.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DMX.Controllers
{
    [Authorize]
    public class ClassroomController : Controller
    {
        private readonly XContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly INotyfService _notyfService;
        private readonly IAuthorizationService _authorizationService;
        private readonly EntityService _entityService;

        public ClassroomController(
            XContext context,
            UserManager<AppUser> userManager,
            INotyfService notyfService,
            IAuthorizationService authorizationService,
            EntityService entityService)
        {
            _context = context;
            _userManager = userManager;
            _notyfService = notyfService;
            _authorizationService = authorizationService;
            _entityService = entityService;
        }

        // GET: View all classrooms
        [HttpGet]
        public IActionResult ViewClassrooms() => ViewComponent(nameof(ViewClassrooms));

        // GET: Add a new classroom
        [HttpGet]
        public IActionResult AddClassroom() => ViewComponent(nameof(AddClassroom));

        // POST: Add a new classroom
        [HttpPost]
        public async Task<IActionResult> AddClassroom(AddClassroomVM addClassroomVm)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Invalid data. Please check the form and try again.", 5);
                return RedirectToAction(nameof(ViewClassrooms));
            }

            try
            {
                var newClassroom = new Classroom
                {
                    Name = addClassroomVm.Name,
                    Capacity = addClassroomVm.Capacity
                };

                bool result = await _entityService.AddEntityAsync(newClassroom, User);
                if (!result)
                {
                    _notyfService.Error("Failed to add the classroom. Please try again.", 5);
                    return RedirectToAction(nameof(ViewClassrooms));
                }

                _notyfService.Success("Classroom added successfully.", 5);
                return RedirectToAction(nameof(ViewClassrooms));
            }
            catch (Exception ex)
            {
                _notyfService.Error("An error occurred: " + ex.Message, 5);
                return RedirectToAction("Error", "Home", new { message = "An error occurred while adding the classroom." });
            }
        }

        // GET: Edit a classroom
        [HttpGet]
        public async Task<IActionResult> EditClassroomAsync(string id)
        {
            var decryptedId = Encryption.Decrypt(id);
            var classroom = await _context.Classrooms.FirstOrDefaultAsync(m => m.ClassroomId == decryptedId);
            if (classroom == null)
            {
                return NotFound();
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, classroom, "ClassroomOwnerPolicy");
            if (!authorizationResult.Succeeded)
            {
                _notyfService.Error("You do not have access to this resource!", 5);
                return Json(new { success = false, message = "You do not have access to this resource!" });
            }

            return ViewComponent("EditClassroom", id);
        }

        // POST: Edit a classroom
        [HttpPost]
        public async Task<IActionResult> EditClassroom(string id, EditClassroomVM editClassroomVm)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Invalid data. Please check the form and try again.", 5);
                return RedirectToAction(nameof(ViewClassrooms));
            }

            try
            {
                var decryptedId = Encryption.Decrypt(id);
                var classroomToUpdate = await _context.Classrooms.FirstOrDefaultAsync(m => m.ClassroomId == decryptedId);
                if (classroomToUpdate == null)
                {
                    _notyfService.Error("Classroom not found.", 5);
                    return RedirectToAction(nameof(ViewClassrooms));
                }

                classroomToUpdate.Name = editClassroomVm.Name;
                classroomToUpdate.Capacity = editClassroomVm.Capacity;

                bool isEdited = await _entityService.EditEntityAsync(classroomToUpdate, User);
                if (!isEdited)
                {
                    _notyfService.Error("Failed to update the classroom. Please try again.", 5);
                    return RedirectToAction(nameof(ViewClassrooms));
                }

                _notyfService.Success("Classroom updated successfully.", 5);
                return RedirectToAction(nameof(ViewClassrooms));
            }
            catch (Exception ex)
            {
                _notyfService.Error("An unexpected error occurred. Please try again.", 5);
                Console.WriteLine($"Error updating classroom: {ex.Message}");
                return RedirectToAction("Error", "Home", new { message = "An error occurred while updating the classroom." });
            }
        }

        // POST: Delete a classroom
        [HttpPost]
        public async Task<IActionResult> DeleteClassroom(string id)
        {
            try
            {
                var decryptedId = Encryption.Decrypt(id);
                var classroomToDelete = await _context.Classrooms.FirstOrDefaultAsync(m => m.ClassroomId == decryptedId);
                if (classroomToDelete == null)
                {
                    return NotFound();
                }

                // Check if the classroom is assigned to any group
                var isClassroomAssigned = await _context.Groups.AnyAsync(g => g.ClassroomId == decryptedId);
                if (isClassroomAssigned)
                {
                    _notyfService.Error("Cannot delete the classroom because it is assigned to a group.", 5);
                    return RedirectToAction(nameof(ViewClassrooms));
                }

                bool isDeleted = await _entityService.DeleteEntityAsync(classroomToDelete, User);
                if (isDeleted)
                {
                    _notyfService.Success("Classroom deleted successfully.", 5);
                }
                else
                {
                    _notyfService.Error("Failed to delete the classroom. Please try again.", 5);
                }

                return RedirectToAction(nameof(ViewClassrooms));
            }
            catch (Exception ex)
            {
                _notyfService.Error("An error occurred: " + ex.Message, 5);
                return RedirectToAction("Error", "Home", new { message = "An error occurred while deleting the classroom." });
            }
        }

        // GET: View classroom details
        [HttpGet]
        public IActionResult DetailClassroom(string id) => ViewComponent(nameof(DetailClassroom), id);

        // GET: Print classroom details
        [HttpGet]
        public IActionResult PrintClassroom(string id) => ViewComponent(nameof(PrintClassroom), id);

        // GET: Comment on a classroom
        [HttpGet]
        public IActionResult CommentClassroom(string id) => ViewComponent(nameof(CommentClassroom), id);
    }
}