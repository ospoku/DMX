using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using AspNetCoreHero.ToastNotification.Abstractions;
using DMX.ViewModels;
using DMX.Data;
using DMX.Models;
using Microsoft.AspNetCore.Authorization;
using DMX.DataProtection;


namespace DMX.Controllers
{
    
    public class LetterController(XContext dContext, UserManager<AppUser> userManager, INotyfService notyfService
           ) : Controller
    {
        public readonly UserManager<AppUser> usm = userManager;
        public readonly XContext dcx = dContext;
        private readonly INotyfService notyf = notyfService;

        [HttpGet]
        public IActionResult AddLetter() => ViewComponent("AddLetter");
        [HttpGet]
        public IActionResult AddPatient() => ViewComponent("AddPatient");
        [HttpPost]
        public async Task<IActionResult> AddPatient(AddPatientVM addPatientVM)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Document addition error!!! Please try again";

                return ViewComponent("AddPatient");
            }

            if (ModelState.IsValid)
            {
                Patient addThisPatient = new()
                {
                    Date = addPatientVM.Date,
                    FinalDiagnoses = addPatientVM.FinalDiagnoses,

                    WardInCharge = addPatientVM.WardInCharge,

                    IsDeleted = false,
                    CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Name").Value,
                    CreatedDate = DateTime.Now,
                };
                dcx.Patients.Add(addThisPatient);

                dcx.MemoAssignments.Add(new MemoAssignment
                {
                   // SelectedUsers = string.Join(',', addPatientVM.SelectedUsers),

                });


                if (await dcx.SaveChangesAsync(userId: User?.FindFirst(c => c.Type == "Name").Value) > 0)
                {
                    notyf.Success("Client successfully created.");
                    return RedirectToAction("ViewPatients");

                }
                else
                {
                    notyf.Error("Member creation error!!! Please try again");
                }
                return RedirectToAction("AddPatient");

            }
            else
            {
                return RedirectToAction("AddPatient");
            }


        }
                
       
        [HttpPost]
        public async Task<IActionResult> AddLetter(AddLetterVM addDocumentVM, IFormFile formFile)
        {


            


            Letter addThisDocument = new()
            {
                Source = addDocumentVM.DocumentSource,
                DateReceived = addDocumentVM.ReceiptDate,
                DocumentDate = addDocumentVM.DocumentDate,
                ReferenceNumber = addDocumentVM.ReferenceNumber,
                IsDeleted = false,
                CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Name").Value,
                CreatedDate = DateTime.UtcNow,
                AdditionalNotes=addDocumentVM.AdditionalNotes,

            };
            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);
                addThisDocument.PDF = memoryStream.ToArray();
            }
            dcx.Letters.Add(addThisDocument);

            //dcx.MemoAssignments.Add(new Assignment
            //{
            //    TaskId = addThisDocument.LetterId,
            //    SelectedUsers = string.Join(',', addDocumentVM.SelectedUsers),
            //});
            if (await dcx.SaveChangesAsync(User?.FindFirst(c => c.Type == "Name").Value) > 0)
            {
                notyf.Success("Document successfully saved!!!", 5);

                ViewBag.Message = "Document successfully added";
                return RedirectToAction("ViewLetters");
            }
            else
            {
                ViewBag.Message = "Failed to save document";
                notyf.Error("Document saving failed");
                return ViewComponent("AddDocument");

            }
        }

        public IActionResult DetailDocument(string Id)
      => ViewComponent("DetailDocument", Id);
        public IActionResult EditDocument(string Id)
        => ViewComponent("EditDocument", Id);
        [HttpPost]
        public async Task<IActionResult> EditDocumentAsync(string Id, Letter document, IFormFile formFile)
        {
            EditDocumentVM edvm = new();
            Letter updateThisDocument = new();
            updateThisDocument = (from a in dcx.Letters where a.LetterId == Id select a).FirstOrDefault();
            updateThisDocument.ReferenceNumber = document.ReferenceNumber;
            updateThisDocument.DocumentDate = document.DocumentDate;
            updateThisDocument.DateReceived = document.DateReceived;
            updateThisDocument.Source = document.Source;
            updateThisDocument.DateReceived = document.DateReceived;
            updateThisDocument.IsDeleted = false;
            updateThisDocument.ModifiedBy = User.Claims.FirstOrDefault(c => c.Type == "Name").Value;
            updateThisDocument.ModifiedDate = DateTime.Now;

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
                return RedirectToAction("ViewDocuments");
            }
            else
            {
                ViewBag.Message = "Failed to save document";
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
        public async Task<IActionResult> AddLetterComment(string Id, DocumentCommentVM addDocumentCommentVM)
        {

            Letter letterToComment = new();
            letterToComment = (from l in dcx.Letters where l.LetterId == Id select l).FirstOrDefault();

            LetterComment addThisComment = new()
            {
                LetterId = letterToComment.LetterId,
                CreatedDate = DateTime.Now,

                Message = addDocumentCommentVM.NewComment,


                CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Name").Value,
                  UserId = usm.FindByNameAsync(User.Claims.FirstOrDefault(c => c.Type == "Name").Value).Result.Id,
            };

            dcx.LetterComments.Add(addThisComment);
            await dcx.SaveChangesAsync();

            return RedirectToAction("ViewDocuments");
        }
        private byte[] ConvertToBytes(IFormFile file)
        {

            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            };


        }
        [HttpGet]
        public IActionResult CommentDocument(string Id)
        {
            return ViewComponent("CommentDocument", Id);
        }

        [HttpGet]
        public IActionResult PrintDocument(string Id)
        {


            return ViewComponent("PrintDocument", Id);
        }


               [HttpPost]
        public async Task<IActionResult> LetterComment(string Id, DocumentCommentVM addDocumentCommentVM)
        {

            Letter letterToComment = new();
            letterToComment = (from a in dcx.Letters where a.LetterId == Id select a).FirstOrDefault();

            LetterComment addThisComment = new()
            {
                LetterId = letterToComment.LetterId,
                CreatedDate = DateTime.Now,

                Message = addDocumentCommentVM.NewComment,


                CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Name").Value,
                  UserId = usm.FindByNameAsync(User.Claims.FirstOrDefault(c => c.Type == "Name").Value).Result.Id,
            };

            dcx.LetterComments.Add(addThisComment);
            await dcx.SaveChangesAsync();

            return RedirectToAction("ViewMemos");
        }
                
        
        public IActionResult Card(string Id)
        {
            return ViewComponent("Card", Id);
        }

        public IActionResult ViewAssignments()
        {
            return ViewComponent("ViewAssignments");
        }


      
       

        public IActionResult ViewMaternityLeaves()
        {
            return ViewComponent("ViewTravelRequests");
        }


        [HttpGet]
        public async Task<IActionResult> Download(string Id)
        {
            var foundDoc = await dcx.Letters.FirstOrDefaultAsync(m => m.LetterId == Id);
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

