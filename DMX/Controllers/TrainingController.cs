using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using CsvHelper;
using CsvHelper.Configuration;
using DMX.Data;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Globalization;
namespace DMX.Controllers
{
    public class TrainingController(XContext context, INotyfService notyfService) : Controller
    {
        public readonly XContext dcx = context;
        public readonly INotyfService notyf = notyfService;
        public IActionResult ViewExternalTrainings()
         => ViewComponent("ViewExternalTrainings");
        public IActionResult ViewParticipants()
      => ViewComponent("ViewParticipants");


        [HttpGet]
        public IActionResult AddInternalTraining() => ViewComponent("AddInternalTraining");
        [HttpPost]
        public async Task< IActionResult> AddExternalTraining(AddExternalTrainingVM addExternalTrainingVM)
        {
            ExternalTraining addThisExternalTraining = new()
            {
                Attendee = addExternalTrainingVM.StaffId,

                WorkshopTitle = addExternalTrainingVM.WorkshopTitle,

                NumberofDays = addExternalTrainingVM.NumberofDays,
                DepartureDate = addExternalTrainingVM.DepartureDate,
                ReturnDate = addExternalTrainingVM.ReturnDate,
                ProposedTrainingDate = addExternalTrainingVM.ProposedTrainingDate,
                ProposedTrainingGroup = addExternalTrainingVM.ProposedTrainingGroup,
                Description = addExternalTrainingVM.Description,
            };
            dcx.ExternalTrainings.Add(addThisExternalTraining);

            await dcx.SaveChangesAsync();
            if (await dcx.SaveChangesAsync(User?.FindFirst(c => c.Type == "Name").Value) > 0)
            {
                notyf.Success("Client successfully created.");
                return RedirectToAction("ViewMeetings");

            }
            else
            {
                notyf.Error("Member creation error!!! Please try again");
            }
            return RedirectToAction("AddMeeting");
        }
      

        [HttpGet]
        public IActionResult AddExternalTraining()
         => ViewComponent("AddExternalTraining");
        public IActionResult ViewAttendance()
    => ViewComponent("ViewAttendances");
        [HttpGet]
        public IActionResult ViewInternalTrainings()
    => ViewComponent("ViewInternalTrainings");
        [HttpPost]
        public async Task<IActionResult> AddInternalTraining(AddInternalTrainingVM addTrainingVM)
        {
            InternalTraining addThisTraining = new()
            {
                EventName = addTrainingVM.Name,
                Date = addTrainingVM.Date,
                Description = addTrainingVM.Description,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Name").Value,
            };

            dcx.InternalTrainings.Add(addThisTraining);
            await dcx.SaveChangesAsync();

            return RedirectToAction("ViewInternalTrainings");
        }
        [HttpGet]
        public IActionResult AddAttendance(string Id) => ViewComponent("AddAttendance", Id);
        [HttpPost]
        public async Task<IActionResult> ManageAttendance(string Id, AttendanceVM attVM)
        {
            if (ModelState.IsValid)
            {
                InternalTraining itr = dcx.InternalTrainings.SingleOrDefault(i => i.TrainingId == Id);

                foreach(var attendee in attVM.Participants)
                {
                    Attendance addThisAttendnace = new()
                    {
                        CreatedDate = DateTime.UtcNow,
                        ParticipantId = attendee.Value,
                        EventId = Id,

                    };
                }

               
             
                await dcx.SaveChangesAsync();
                if (await dcx.SaveChangesAsync( User?.FindFirst(c => c.Type == "Name").Value) > 0)
                {
                    notyf.Success("Client successfully created.");
                    return RedirectToAction("ViewMeetings");

                }
                else
                {
                    notyf.Error("Member creation error!!! Please try again");
                }
                return RedirectToAction("AddMeeting");




            }



            return ViewComponent("ViewAttendances");
        }


        [HttpGet]
        public IActionResult AddMeeting()
        {
            return ViewComponent("AddMeeting");
        }

        [HttpPost]
        public async Task< IActionResult> AddMeeting(AddMeetingVM addMeetingVM)
        {
            Meeting addThisMeeting = new()
            { Name=addMeetingVM.Name,
                Description = addMeetingVM.Description,
               CreatedDate = DateTime.Now,
                CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Name").Value
            };
            dcx.Meetings.Add(addThisMeeting);
        
            if (await dcx.SaveChangesAsync(userId: User?.FindFirst(c => c.Type == "Name").Value) > 0)
            {
                notyf.Success("Client successfully created.");
                return RedirectToAction("ViewMeetings");

            }
            else
            {
                notyf.Error("Member creation error!!! Please try again");
            }
            return RedirectToAction("AddMeeting");



       
        }

        [HttpGet]
        public IActionResult ViewMeetings()
        {
            return ViewComponent("ViewMeetings");
        }

        [HttpGet]
        public IActionResult AddWorkshop()
        {
            return ViewComponent("AddWorkshop");
        }
        [HttpGet]
        public IActionResult ViewWorkshops()
        {
            return ViewComponent("ViewWorkshops");
        }


        //[HttpGet]
        //public IActionResult ImportFromStaffList()
        //{
        //    return ViewComponent("ImportFromStaffList");
        //}
        //[HttpPost]
        //public async Task<IActionResult> ImportFromStaffList(ImportFromStaffListVM importFromStaffListVM)
        //{

        //    foreach (var staff in importFromStaffListVM.SelectedId)
        //    {

        //        var selectedStaff = dcx.StaffList.Where(x => x.StaffId.Contains(staff.ToString())).ToList();

        //        foreach (var staffid in selectedStaff)
        //        {
        //            Participant newParticipant = new()
        //            {
        //                DateOfBirth = staffid.DateOfBirth,
        //                Contact = staffid.Contact,
        //                Name = staffid.Name,
        //                Rank = staffid.Rank,
        //                Department = staffid.Department,
        //            };

        //            dcx.Participants.Add(newParticipant);
        //            await dcx.SaveChangesAsync();
        //        }

        //    };



        //    return ViewComponent("ViewParticipants");
        //}
    //    [HttpGet]
    //    public IActionResult ImportFromExcel()
    //    {
    //        return ViewComponent("ImportFromExcel");
    //    }
    //    [HttpPost]
    //    public async Task<IActionResult> ImportFromExcel(IFormFile formFile)
    //    {
    //        var data = new MemoryStream();
    //        await formFile.CopyToAsync(data);

    //        data.Position = 0;
    //        using (var reader = new StreamReader(data))
    //        {
    //            var bad = new List<string>();
    //            var conf = new CsvConfiguration(CultureInfo.InvariantCulture)
    //            {
    //                HasHeaderRecord = true,
    //                HeaderValidated = null,
    //                MissingFieldFound = null,
    //                DetectColumnCountChanges = true,
    //                InjectionOptions= InjectionOptions.Exception,

    //                BadDataFound = context =>
    //                {
    //                    bad.Add(context.RawRecord);
    //                }
    //            };
    //            using (var csvReader = new CsvReader(reader, conf))
    //            {
    //                while (csvReader.Read())
    //                {
    //                    var Name = csvReader.GetField(0).ToString();
    //                    var Contact = csvReader.GetField(1).ToString();
    //                    var DoB = csvReader.GetField(2);
    //                    var Department = csvReader.GetField(3).ToString();

    //                    //await dcx.Participants.AddAsync(new Participant
    //                    //{
    //                    //    Name = Name.ToString(),
    //                    //    Contact = Contact,
    //                    //    Department = Department,
    //                    //    DateOfBirth = DateTime.Parse(DoB),

    //                    //});
    //                    dcx.SaveChanges();
    //                }
    //            };
    //        }
    //        return ViewComponent("ViewParticipants");
    //    }
    }
}



