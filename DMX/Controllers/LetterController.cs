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
        public IActionResult AddDocument() => ViewComponent("AddDocument");
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

                dcx.Assignments.Add(new Assignment
                {
                    SelectedUsers = string.Join(',', addPatientVM.SelectedUsers),

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
        public async Task<IActionResult> AddDocument(AddDocumentVM addDocumentVM, IFormFile formFile)
        {


            if (!ModelState.IsValid)
            {
                return ViewComponent("AddDocument");
            }


            Letter addThisDocument = new()
            {
                DocumentSource = addDocumentVM.DocumentSource,
                DateReceived = addDocumentVM.ReceiptDate,
                DocumentDate = addDocumentVM.DocumentDate,
                ReferenceNumber = addDocumentVM.ReferenceNumber,
                IsDeleted = false,
                CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Name").Value,
                CreatedDate = DateTime.UtcNow,


            };
            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);
                addThisDocument.PDF = memoryStream.ToArray();
            }
            dcx.Documents.Add(addThisDocument);

            dcx.Assignments.Add(new Assignment
            {
                TaskId = addThisDocument.DocumentId,
                SelectedUsers = string.Join(',', addDocumentVM.SelectedUsers),
            });
            if (await dcx.SaveChangesAsync(User?.FindFirst(c => c.Type == "Name").Value) > 0)
            {
                notyf.Success("Document successfully saved!!!", 5);

                ViewBag.Message = "Document successfully added";
                return RedirectToAction("ViewDocuments");
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
            updateThisDocument = (from a in dcx.Documents where a.DocumentId == Id select a).FirstOrDefault();
            updateThisDocument.ReferenceNumber = document.ReferenceNumber;
            updateThisDocument.DocumentDate = document.DocumentDate;
            updateThisDocument.DateReceived = document.DateReceived;
            updateThisDocument.DocumentSource = document.DocumentSource;
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
            dcx.Documents.Attach(updateThisDocument);
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
            return ViewComponent("ViewDocuments");
        }

        public IActionResult DeleteDocument() => ViewComponent("ViewDocuments"); [HttpPost]
        public async Task<IActionResult> AddDocumentComment(string Id, DocumentCommentVM addDocumentCommentVM)
        {

            Letter documentToComment = new();
            documentToComment = (from d in dcx.Documents where d.DocumentId == Id select d).FirstOrDefault();

            Comment addThisComment = new()
            {
                TaskId = documentToComment.DocumentId,
                CreatedDate = DateTime.Now,

                Message = addDocumentCommentVM.NewComment,


                CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Name").Value,
                //  UserId = usm.FindByNameAsync(User.Claims.FirstOrDefault(c => c.Type == "Name").Value).Result.Id,
            };

            dcx.Comments.Add(addThisComment);
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
        public async Task<IActionResult> PettyCashComment(string Id, MemoCommentVM addCommentVM)
        {

            Memo memoToUpdate = new();
            memoToUpdate = (from a in dcx.Memos where a.MemoId == Id select a).FirstOrDefault();

            Comment addThisComment = new()
            {
                TaskId = memoToUpdate.MemoId,
                CreatedDate = DateTime.Now,

                Message = addCommentVM.NewComment,


                CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Name").Value,
                //  UserId = usm.FindByNameAsync(User.Claims.FirstOrDefault(c => c.Type == "Name").Value).Result.Id,
            };

            dcx.Comments.Add(addThisComment);
            await dcx.SaveChangesAsync();

            return RedirectToAction("ViewMemos");
        }
        [HttpPost]
        public async Task<IActionResult> DocumentComment(string Id, DocumentCommentVM addDocumentCommentVM)
        {

            Letter documentToComment = new();
            documentToComment = (from a in dcx.Documents where a.DocumentId == Id select a).FirstOrDefault();

            Comment addThisComment = new()
            {
                TaskId = documentToComment.DocumentId,
                CreatedDate = DateTime.Now,

                Message = addDocumentCommentVM.NewComment,


                CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Name").Value,
                //  UserId = usm.FindByNameAsync(User.Claims.FirstOrDefault(c => c.Type == "Name").Value).Result.Id,
            };

            dcx.Comments.Add(addThisComment);
            await dcx.SaveChangesAsync();

            return RedirectToAction("ViewMemos");
        }
                [HttpPost]
        public async Task<IActionResult> PatientComment(string Id, MemoCommentVM addCommentVM)
        {

            Memo memoToUpdate = new();
            memoToUpdate = (from a in dcx.Memos where a.MemoId == Id select a).FirstOrDefault();

            Comment addThisComment = new()
            {
                TaskId = memoToUpdate.MemoId,
                CreatedDate = DateTime.Now,

                Message = addCommentVM.NewComment,


                CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Name").Value,
                //  UserId = usm.FindByNameAsync(User.Claims.FirstOrDefault(c => c.Type == "Name").Value).Result.Id,
            };

            dcx.Comments.Add(addThisComment);
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
            var foundDoc = await dcx.Documents.FirstOrDefaultAsync(m => m.DocumentId == Id);
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

