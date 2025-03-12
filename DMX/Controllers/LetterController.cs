using AspNetCoreHero.ToastNotification.Abstractions;
using DMX.Data;
using DMX.DataProtection;
using DMX.Models;
using DMX.Services;
using DMX.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DMX.Controllers
{
    [Authorize]
    public class LetterController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly XContext _context;
        private readonly INotyfService _notyfService;
        private readonly EntityService _entityService;

        public LetterController(
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
        public IActionResult AddLetter() => ViewComponent("AddLetter");

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 104857600)] // 100MB limit
        public async Task<IActionResult> AddLetter(AddLetterVM addLetterVm, IFormFile formFile)
        {
            if (addLetterVm.SelectedUsers == null || !addLetterVm.SelectedUsers.Any())
            {
                _notyfService.Error("You must select at least one user for assignment.", 5);
                return RedirectToAction("ViewLetters");
            }

            try
            {
                if (formFile == null || formFile.Length == 0)
                {
                    _notyfService.Error("Please upload a valid document.");
                    return RedirectToAction("ViewLetters");
                }

                if (Path.GetExtension(formFile.FileName).ToLower() != ".pdf")
                {
                    _notyfService.Error("Only PDF documents are allowed.");
                    return RedirectToAction("ViewLetters");
                }

                var existingLetter = await _context.Letters.FirstOrDefaultAsync(l =>
                    l.Subject.ToLower() == addLetterVm.Subject.ToLower() &&
                    l.Source.ToLower() == addLetterVm.Source.ToLower() &&
                    l.DateReceived == addLetterVm.ReceiptDate &&
                    l.DocumentDate == addLetterVm.DocumentDate);

                if (existingLetter != null)
                {
                    _notyfService.Error("This record already exists.");
                    return RedirectToAction("ViewLetters");
                }

                var newLetter = new Letter
                {
                    Source = addLetterVm.Source,
                    Subject = addLetterVm.Subject,
                    DateReceived = addLetterVm.ReceiptDate,
                    DocumentDate = addLetterVm.DocumentDate,
                    AdditionalNotes = addLetterVm.AdditionalNotes
                };

                using (var memoryStream = new MemoryStream())
                {
                    await formFile.CopyToAsync(memoryStream);
                    newLetter.PDF = memoryStream.ToArray();
                }

                bool result = await _entityService.AddEntityAsync(newLetter, User);
                if (result)
                {
                    _notyfService.Success("Record successfully saved!!!", 5);
                    return RedirectToAction("ViewLetters");
                }
                else
                {
                    _notyfService.Error("Document saving failed.");
                    return RedirectToAction("ViewLetters");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { message = "An error occurred while processing the request: " + ex.Message });
            }
        }

        [HttpGet]
        public IActionResult EditLetter(string Id) => ViewComponent(nameof(EditLetter), Id);

        [HttpPost]
        public async Task<IActionResult> EditLetterAsync(string id, Letter letter, IFormFile formFile)
        {
            try
            {
                var letterToUpdate = await _context.Letters.FirstOrDefaultAsync(a => a.LetterId == id);
                if (letterToUpdate == null)
                {
                    return NotFound();
                }

                letterToUpdate.ReferenceNumber = letter.ReferenceNumber;
                letterToUpdate.DocumentDate = letter.DocumentDate;
                letterToUpdate.DateReceived = letter.DateReceived;
                letterToUpdate.Source = letter.Source;
                letterToUpdate.AdditionalNotes = letter.AdditionalNotes;

                if (formFile != null && formFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await formFile.CopyToAsync(memoryStream);
                        letterToUpdate.PDF = memoryStream.ToArray();
                    }
                }

                _context.Letters.Attach(letterToUpdate);
                _context.Entry(letterToUpdate).State = EntityState.Modified;

                if (await _context.SaveChangesAsync() > 0)
                {
                    _notyfService.Success("Record successfully updated.");
                    return RedirectToAction("ViewLetters");
                }
                else
                {
                    _notyfService.Error("Document saving failed.");
                    return ViewComponent("AddDocument");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { message = "An error occurred while updating the document: " + ex.Message });
            }
        }

        [HttpGet]
        public IActionResult ViewLetters() => ViewComponent("ViewLetters");

        [HttpGet]
        public IActionResult DeleteLetter() => ViewComponent("ViewLetters");

        [HttpPost]
        public async Task<IActionResult> CommentLetter(string id, DocumentCommentVM commentVm)
        {
            try
            {
                var decryptedId = Encryption.Decrypt(id);
                var letterToComment = await _context.Letters.FirstOrDefaultAsync(l => l.LetterId == decryptedId);
                if (letterToComment == null)
                {
                    return NotFound();
                }

                var newComment = new LetterComment
                {
                    LetterId = letterToComment.LetterId,
                    Message = commentVm.NewComment,
                    UserId = (await _userManager.GetUserAsync(User)).Id
                };

                bool result = await _entityService.AddEntityAsync(newComment, User);
                if (result)
                {
                    _notyfService.Success("Comment successfully saved!!!");
                }
                else
                {
                    _notyfService.Error("Comment could not be saved.");
                }

                return RedirectToAction("ViewLetters");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { message = "An error occurred while processing the comment: " + ex.Message });
            }
        }

        [HttpGet]
        public IActionResult CommentLetter(string id) => ViewComponent("CommentLetter", id);

        [HttpGet]
        public IActionResult PrintDocument(string id) => ViewComponent("PrintDocument", id);

        [HttpGet]
        public async Task<IActionResult> Download(string id)
        {
            var decryptedId = Encryption.Decrypt(id);
            var letter = await _context.Letters.FirstOrDefaultAsync(m => m.LetterId == decryptedId);
            if (letter == null || letter.PDF == null)
            {
                return NotFound();
            }

            return new FileContentResult(letter.PDF, "application/octet-stream")
            {
                FileDownloadName = $"Document_{letter.ReferenceNumber}.pdf"
            };
        }
    }
}