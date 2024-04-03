using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using AspNetCoreHero.ToastNotification.Abstractions;
using DMX.ViewModels;
using DMX.Data;
using DMX.Models;
using Microsoft.AspNetCore.Authorization;


namespace DMX.Controllers
{
    
    public class DocumentController(XContext dContext, UserManager<AppUser> userManager, INotyfService notyfService
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
                    //SelectedIdArray=  string.Join(',', addPatientVM.SelectedUsers),

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
                return ViewComponent("AddClient");

            }
            else
            {
                return ViewComponent("AddPatient");
            }


        }

        private byte[] ConvertToBytes(IFormFile file)
        {

            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            };


        }
        [HttpPost]
        public async Task<IActionResult> AddDocument(AddDocumentVM addDocumentVM, IFormFile formFile)
        {


            if (!ModelState.IsValid)
            {
                return ViewComponent("AddDocument");
            }


            Document addThisDocument = new()
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
                return ViewComponent("ViewDocuments");
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
        public IActionResult EditMemo(string Id)
        => ViewComponent("EditMemo", Id);
        [HttpPost]
        public async Task<IActionResult> EditDocumentAsync(string Id, Document document, IFormFile formFile)
        {
            EditDocumentVM edvm = new EditDocumentVM();
            Document updateThisDocument = new();
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
       
        public IActionResult ViewDocuments()
        {
            return ViewComponent("ViewDocuments");
        }

        public IActionResult DeleteDocument() => ViewComponent("ViewDocuments");

        public IActionResult ViewMemos()
        {
            return ViewComponent("ViewMemos");
        }
        [HttpGet]
        public IActionResult AddMemo()
        {
            return ViewComponent("AddMemo");
        }
        [HttpPost]
        public async Task<IActionResult> AddMemo(AddMemoVM addMemoVM)
        {
            Memo addThisMemo = new Memo()
            {
                Content = addMemoVM.Content,
                To = addMemoVM.To,
                From = addMemoVM.From,
                Title = addMemoVM.Title,
                CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Name").Value,
                CreatedDate = DateTime.UtcNow,

            };
            dcx.Memos.Add(addThisMemo);

            dcx.Assignments.Add(new Assignment
            {
                TaskId = addThisMemo.MemoId,
                SelectedUsers = string.Join(',', addMemoVM.SelectedUsers),
                CreatedBy= User.Claims.FirstOrDefault(c => c.Type == "Name").Value,
                CreatedDate = DateTime.UtcNow,
            });



            if (await dcx.SaveChangesAsync(User.Claims.FirstOrDefault(c => c.Type == "Name").Value) > 0)
            {
                notyf.Success("Memo successfully saved", 5);


            }
            else
            {
                notyf.Error("Error, Memo could not be saved!!!", 5);
            }

            return ViewComponent("ViewMemos");
        }
        [HttpGet]
        public IActionResult EditPatient(string Id)
        {
            return ViewComponent("EditPatient", Id);
        }
        [HttpGet]
        public IActionResult CommentMemo(string Id)
        {
            return ViewComponent("CommentMemo", Id);
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
        public async Task<IActionResult> EditMemo(string Id, EditMemoVM editMemoVM)
        {

            Memo updateThisMemo = (from a in dcx.Memos where a.MemoId == Id select a).FirstOrDefault();


            updateThisMemo.Content = editMemoVM.Content;
            updateThisMemo.ModifiedDate = DateTime.UtcNow;

            updateThisMemo.Title = editMemoVM.Title;


            updateThisMemo.ModifiedBy = User.Claims.FirstOrDefault(c => c.Type == "Name").Value;
            //updateThisMemo.UserId = usm.FindByNameAsync(User.Claims.FirstOrDefault(c => c.Type == "Name").Value).Result.Id;

            Assignment updateThisAssignment = (from x in dcx.Assignments where x.TaskId == Id select x).FirstOrDefault();

            updateThisAssignment.SelectedUsers = string.Join(',', editMemoVM.SelectedUsers);



            dcx.Memos.Attach(updateThisMemo);

            dcx.Entry(updateThisMemo).State = EntityState.Modified;
            if (await dcx.SaveChangesAsync() > 0)
            {

                return RedirectToAction("ViewMemos");
            }
            else
            {
                return ViewComponent("EditMemo");
            }

        }

        [HttpPost]
        public async Task<IActionResult> MemoComment(string Id, MemoCommentVM addCommentVM)
        {

            Memo memoToUpdate = new Memo();
            memoToUpdate = (from a in dcx.Memos where a.MemoId == Id select a).FirstOrDefault();

            Comment addThisComment = new Comment
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
        public async Task<IActionResult> AddDocumentComment(string Id, DocumentCommentVM addDocumentCommentVM)
        {

            Document documentToComment = new Document();
            documentToComment = (from d in dcx.Documents where d.DocumentId == Id select d).FirstOrDefault();

            Comment addThisComment = new Comment
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
        [HttpPost]
        public async Task<IActionResult> LeaveComment(string Id, MemoCommentVM addCommentVM)
        {

            Memo memoToUpdate = new Memo();
            memoToUpdate = (from a in dcx.Memos where a.MemoId == Id select a).FirstOrDefault();

            Comment addThisComment = new Comment
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
        public async Task<IActionResult> TravelRequestComment(string Id, MemoCommentVM addCommentVM)
        {

            Memo memoToUpdate = new Memo();
            memoToUpdate = (from a in dcx.Memos where a.MemoId == Id select a).FirstOrDefault();

            Comment addThisComment = new Comment
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
        public async Task<IActionResult> MaternityLeaveComment(string Id, MemoCommentVM addCommentVM)
        {

            Memo memoToUpdate = new Memo();
            memoToUpdate = (from a in dcx.Memos where a.MemoId == Id select a).FirstOrDefault();

            Comment addThisComment = new Comment
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
        public async Task<IActionResult> PettyCashComment(string Id, MemoCommentVM addCommentVM)
        {

            Memo memoToUpdate = new Memo();
            memoToUpdate = (from a in dcx.Memos where a.MemoId == Id select a).FirstOrDefault();

            Comment addThisComment = new Comment
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
        public async Task<IActionResult> ExcuseDutyComment(string Id, MemoCommentVM addCommentVM)
        {

            Memo memoToUpdate = new Memo();
            memoToUpdate = (from a in dcx.Memos where a.MemoId == Id select a).FirstOrDefault();

            Comment addThisComment = new Comment
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

            Document documentToComment = new Document();
            documentToComment = (from a in dcx.Documents where a.DocumentId == Id select a).FirstOrDefault();

            Comment addThisComment = new Comment
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
        public async Task<IActionResult> ServiceRequestComment(string Id, MemoCommentVM addCommentVM)
        {

            Memo memoToUpdate = new Memo();
            memoToUpdate = (from a in dcx.Memos where a.MemoId == Id select a).FirstOrDefault();

            Comment addThisComment = new Comment
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
        public async Task<IActionResult> PatientComment(string Id, MemoCommentVM addCommentVM)
        {

            Memo memoToUpdate = new Memo();
            memoToUpdate = (from a in dcx.Memos where a.MemoId == Id select a).FirstOrDefault();

            Comment addThisComment = new Comment
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
        public IActionResult ViewServiceRequests()
        {
            return ViewComponent("ViewServiceRequests");
        }
        [HttpGet]
        public IActionResult AddServiceRequest()
        {
            return ViewComponent("AddServiceRequest");
        }
        [HttpPost]
        public async Task<IActionResult> AddServiceRequest(AddServiceRequestVM addServiceRequestVM)
        {
            ServiceRequest addThisServiceRequest = new ServiceRequest
            {
                ActionToBeTaken = addServiceRequestVM.ActionToBeTaken,
                FaultInspectedBy = addServiceRequestVM.FaultInspectedBy,
                Faults = addServiceRequestVM.Faults,
            };
            dcx.ServiceRequests.Add(addThisServiceRequest);

            if (await dcx.SaveChangesAsync() > 0)
            {
                return RedirectToAction("ViewServiceRequests");
            }
            else
            {
                return ViewComponent("AddServiceRequest");
            }

        }

        [HttpGet]
        public IActionResult AddExcuseDuty()
        {
            return ViewComponent("AddExcuseDuty");
        }
        [HttpPost]
        public async Task<IActionResult> AddServiceResquests(AddServiceRequestVM addServiceRequestVM)
        {
            ServiceRequest addThisServiceRequest = new ServiceRequest()
            {
                RequestedBy = addServiceRequestVM.ServiceRequestedBy,

            };
            dcx.ServiceRequests.Add(addThisServiceRequest);
            await dcx.SaveChangesAsync();
            return ViewComponent("ViewServiceRequests");
        }
        public IActionResult EditServiceRequest(string Id)
        {
            return ViewComponent("EditMemo", Id);
        }
        [HttpPost]
        public async Task<IActionResult> EditServiceRequest(string Id, EditServiceRequestVM editServiceRequestVM)
        {

            ServiceRequest serviceRequestToComment = new ServiceRequest();
            serviceRequestToComment = (from s in dcx.ServiceRequests where s.ServiceRequestId == Id select s).FirstOrDefault();

            Comment addThisComment = new Comment
            {
                //MemoId = memoToUpdate.MemoId,
                CreatedDate = DateTime.UtcNow,




                CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Name").Value,
                //UserId = usm.FindByNameAsync(User.Claims.FirstOrDefault(c => c.Type == "Name").Value).Result.Id,
            };

            dcx.Comments.Add(addThisComment);
            await dcx.SaveChangesAsync();

            return RedirectToAction("ViewMemos");
        }

        public IActionResult ViewLeaves()
        {
            return ViewComponent("ViewLeaves");
        }
        public IActionResult ViewPatients()
        {
            return ViewComponent("ViewPatients");
        }
        [HttpGet]
        public IActionResult ViewPettyCash()
        {
            return ViewComponent("ViewPettyCash");
        }
        public IActionResult Card(string Id)
        {
            return ViewComponent("Card", Id);
        }

        public IActionResult ViewAssignments()
        {
            return ViewComponent("ViewAssignments");
        }


        [HttpGet]
        public IActionResult AddPettyCash() => ViewComponent("AddPettyCash");
        [HttpGet]
        public IActionResult AddSickReport() => ViewComponent("AddSickReport");
        [HttpGet]
        public IActionResult ViewSickReports() => ViewComponent("ViewSickReports");

        [HttpGet]
        public IActionResult AddTravelRequest() => ViewComponent("AddTravelRequest");
        public IActionResult ViewExcuseDuties()
        {
            return ViewComponent("ViewExcuseDuties");
        }
        [HttpGet]
        public IActionResult ViewTravelRequests()
        {
            return ViewComponent("ViewTravelRequests");
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

