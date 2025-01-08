using AspNetCoreHero.ToastNotification.Abstractions;
using DMX.Data;
using DMX.DataProtection;
using DMX.Models;
using DMX.Services;
using DMX.ViewComponents;
using DMX.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;


namespace DMX.Controllers
{

    public class LetterController(XContext dContext, UserManager<AppUser> userManager, INotyfService notyfService, EntityService entityService
           ) : Controller
    {
        public readonly UserManager<AppUser> usm = userManager;
        public readonly XContext dcx = dContext;
        private readonly INotyfService notyf = notyfService;
        private readonly EntityService entityServ = entityService;

        [HttpGet]
        public IActionResult AddLetter() => ViewComponent("AddLetter");

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 104857600)] // 100MB limit
        public async Task<IActionResult> AddLetter(AddLetterVM addDocumentVM, IFormFile formFile)
        {
            if (addDocumentVM.SelectedUsers == null || !addDocumentVM.SelectedUsers.Any())
            {
                notyf.Error("You must select at least one user for assignment.", 5);
                // Optionally, repopulate the view model and return the form to the user
                //addPatientVM.UsersList = userService.GetAllUsers().Select(u => new SelectListItem { Value = u.Id, Text = u.Name }).ToList();

                return RedirectToAction("ViewLetters"); // Return the form with the error
            }
            try
            {
                // Validate the file
                if (formFile == null || formFile.Length == 0)
                {
                    notyf.Error("Please upload a valid document.");
                    return RedirectToAction("ViewLetters");
                }

                if (Path.GetExtension(formFile.FileName).ToLower() != ".pdf")
                {
                    notyf.Error("Only PDF documents are allowed.");
                    return RedirectToAction("ViewLetters");
                }

                var existingLetter = await dcx.Letters.FirstOrDefaultAsync(l=>
    l.Subject.ToLower() == addDocumentVM.Subject.ToLower() &&
    l.Source.ToLower() == addDocumentVM.Source.ToLower() &&
    l.DateReceived == addDocumentVM.ReceiptDate &&
    l.DocumentDate==addDocumentVM.DocumentDate);

                if (existingLetter != null)
                {
                    notyf.Error("This record already exists.");
                    return RedirectToAction("ViewLetters");
                }


                Letter addThisDocument = new()
                {   Source=addDocumentVM.Source,
                    Subject=addDocumentVM.Subject,
                    DateReceived = addDocumentVM.ReceiptDate,
                    DocumentDate = addDocumentVM.DocumentDate,
                    AdditionalNotes = addDocumentVM.AdditionalNotes,
                    
                };

                using (var memoryStream = new MemoryStream())
                {
                    await formFile.CopyToAsync(memoryStream);
                    addThisDocument.PDF = memoryStream.ToArray(); // Store the file as byte array
                }

                bool result = await entityServ.AddEntityAsync(addThisDocument, User);

                if (result)
                {
                    notyf.Success("Record successfully saved!!!", 5);
                    return RedirectToAction("ViewLetters");
                }
                else
                {
                    notyf.Error("Document saving failed");
                    return RedirectToAction("ViewLetters");
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error occurred while adding the document");
                return RedirectToAction("Error", "Home", new { message = "An error occurred while processing the request." + ex.Message });
            }
        }



        public IActionResult EditLetter(string Id)
        => ViewComponent("EditLetter", Id);
        [HttpPost]
        public async Task<IActionResult> EditLetterAsync(string Id, Letter document, IFormFile formFile)
        {
            EditDocumentVM editDocumentVM = new();
            EditDocumentVM edvm = editDocumentVM;
            Letter updateThisDocument = new();
            updateThisDocument = (from a in dcx.Letters where a.LetterId == Id select a).FirstOrDefault();
            updateThisDocument.ReferenceNumber = document.ReferenceNumber;
            updateThisDocument.DocumentDate = document.DocumentDate;
            updateThisDocument.DateReceived = document.DateReceived;
            updateThisDocument.Source = document.Source;
            updateThisDocument.DateReceived = document.DateReceived;
            updateThisDocument.AdditionalNotes = document.AdditionalNotes;
            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);
                updateThisDocument.PDF = memoryStream.ToArray();
            }
            dcx.Letters.Attach(updateThisDocument);
            dcx.Entry(updateThisDocument).State = EntityState.Modified;
            if (await dcx.SaveChangesAsync(User?.FindFirst(c => c.Type == "Name").Value) > 0)
            {
                notyf.Success("Record successfully updated");
                return RedirectToAction("ViewLetters");
            }
            else
            {
               
                notyf.Error("Document saving failed");
                return ViewComponent("AddDocument");


            }
        }
        [HttpGet]
        public IActionResult ViewLetters()
        {

            return ViewComponent("ViewLetters");
        }

        public IActionResult DeleteLetter() => ViewComponent("ViewLetters");
        [HttpPost]
        public async Task<IActionResult> CommentLetter(string Id, DocumentCommentVM addDocumentCommentVM)
        {

            Letter letterToComment = new();
            letterToComment = (from l in dcx.Letters where l.LetterId == @Encryption.Decrypt(Id) select l).FirstOrDefault();

            LetterComment addThisComment = new()
            {
                LetterId = letterToComment.LetterId,
                Message = addDocumentCommentVM.NewComment,
                UserId = usm.GetUserAsync(HttpContext.User).Result.Id,
            };

            bool addDocument =await entityServ.AddEntityAsync(addThisComment, User);

            if (addDocument)
            {
                notyf.Success("Comment successfully saved!!!");
                return RedirectToAction("ViewLetters");
            }else
                {
                notyf.Error("Comment could not be saved.!!!");
                return RedirectToAction("ViewLetters");

            }
        }
        private static byte[] ConvertToBytes(IFormFile file)
        {

            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            };


        }
        [HttpGet]
        public IActionResult CommentLetter(string Id)
        {
            return ViewComponent("CommentLetter", Id);
        }

        [HttpGet]
        public IActionResult PrintDocument(string Id)
        {


            return ViewComponent("PrintDocument", Id);
        }


        [HttpPost]
        public async Task<IActionResult> LetterComment(string Id, DocumentCommentVM addDocumentCommentVM)
        {
            try {

                Letter letterToComment = new();
                letterToComment = (from a in dcx.Letters where a.LetterId == Id select a).FirstOrDefault();

                LetterComment addThisComment = new()
                {
                    LetterId = letterToComment.LetterId,
                    CreatedDate = DateTime.Now,

                    Message = addDocumentCommentVM.NewComment,


                    CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Name").Value,
                    UserId = (await usm.GetUserAsync(User)).Id,
                };

                bool result = await entityServ.AddEntityAsync(addThisComment, User);
                if (result)
                {
                    notyf.Success("comment successfully saved!!!", 5);
                    return RedirectToAction("ViewLetters");
                }
                else
                {
                    notyf.Error("comment saving failed");
                    return RedirectToAction("ViewLetters");
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error occurred while adding the document");
                return RedirectToAction("Error", "Home", new { message = "An error occurred while processing the request." + ex.Message
                });
            }
        }
    

        [HttpGet]
        public async Task<IActionResult> Download(string Id)
        {
            var foundDoc = await dcx.Letters.FirstOrDefaultAsync(m => m.LetterId == @Encryption.Decrypt(Id));
            if (foundDoc == null)
            {
                return NotFound();
            }

            if (foundDoc.PDF == null)
            {
                return NotFound();
            }
            else
            {
                byte[] byteArr = foundDoc.PDF;
                string mimeType = "application/octet-stream";
                return new FileContentResult(byteArr, mimeType)
                {
                    FileDownloadName = $"Document {foundDoc.ReferenceNumber}.pdf"
                };
            }


        }


    }
}

