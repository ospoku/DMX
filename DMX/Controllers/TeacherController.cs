using AspNetCoreHero.ToastNotification.Abstractions;
using DMX.Data;
using DMX.DataProtection;
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
    public class TeacherController : Controller
    {
        private readonly XContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly INotyfService _notyfService;
        private readonly EntityService _entityService;

        public TeacherController(
            XContext context,
            UserManager<AppUser> userManager,
            INotyfService notyfService,
            EntityService entityService)
        {
            _context = context;
            _userManager = userManager;
            _notyfService = notyfService;
            _entityService = entityService;
        }

        // GET: View all teachers
        [HttpGet]
        public IActionResult ViewTeachers()
        {
            return ViewComponent(nameof(ViewTeachers));
        }

        // GET: Add a new teacher
        [HttpGet]
        public IActionResult AddTeacher() => ViewComponent(nameof(AddTeacher));

        // POST: Add a new teacher
        [HttpPost]
        public async Task<IActionResult> AddTeacher(AddTeacherVM addTeacherVM)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Invalid data. Please check the form and try again.", 5);
                return RedirectToAction(nameof(ViewTeachers));
            }

            try
            {
                // Check if the teacher already exists
                var existingTeacher = await _context.Teachers
                    .FirstOrDefaultAsync(t =>
                        t.Name.ToLower() == addTeacherVM.Name.ToLower() &&
                        t.FacultyId.ToLower() == addTeacherVM.FacultyId.ToLower());

                if (existingTeacher != null)
                {
                    _notyfService.Error("This teacher already exists.", 5);
                    return RedirectToAction(nameof(ViewTeachers));
                }

                // Create a new teacher
                var newTeacher = new Teacher
                {
                    Name = addTeacherVM.Name,
                    FacultyId = addTeacherVM.FacultyId,
                    // Add other properties as needed
                };

                // Add the teacher to the database
                bool result = await _entityService.AddEntityAsync(newTeacher, User);
                if (!result)
                {
                    _notyfService.Error("Failed to add the teacher. Please try again.", 5);
                    return RedirectToAction(nameof(ViewTeachers));
                }

                _notyfService.Success("Teacher added successfully.", 5);
                return RedirectToAction(nameof(ViewTeachers));
            }
            catch (Exception ex)
            {
                _notyfService.Error("An error occurred: " + ex.Message, 5);
                return RedirectToAction("Error", "Home", new { message = "An error occurred while adding the teacher." });
            }
        }

        // GET: Edit a teacher
        [HttpGet]
        public IActionResult EditTeacher(string id)
        {
            return ViewComponent(nameof(EditTeacher), id);
        }

        // Helper method to get user email
        public async Task<string> GetUserEmailAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user?.Email;
        }
    }
}