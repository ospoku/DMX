using DMX.Data;
using DMX.Models;
using DMX.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Globalization;
namespace DMX.Controllers
{
    public class TrainingController(XContext context) : Controller
    {
        public readonly XContext dcx = context;

        public IActionResult ViewExternalTrainings()
         => ViewComponent("ViewExternalTrainings");
        public IActionResult ViewParticipants()
      => ViewComponent("ViewParticipants");


        [HttpGet]
        public IActionResult AddInternalTraining() => ViewComponent("AddInternalTraining");
        [HttpPost]
        public IActionResult AddExternalTraining(AddExternalTraningVM addExternalTrainingVM)
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
            dcx.SaveChangesAsync();
            return View();
        }
        public IActionResult PrintDocument(string Id)
      => ViewComponent("PrintDocument", Id);

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
            InternalTraining addThisTraining = new InternalTraining()
            {
                EventName = addTrainingVM.Name,
                Date = addTrainingVM.Date,
                Description = addTrainingVM.Description,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Name").Value,
            };

            dcx.InternalTrainings.Add(addThisTraining);
            await dcx.SaveChangesAsync();

            return ViewComponent("ViewInternalTrainings");
        }
        [HttpGet]
        public IActionResult ManageAttendance(string Id) => ViewComponent("ManageAttendance", Id);
        [HttpPost]
        public async Task<IActionResult> ManageAttendance(string Id, AttendanceVM attVM)
        {
            if (ModelState.IsValid)
            {
                InternalTraining itr = dcx.InternalTrainings.SingleOrDefault(i => i.TrainingId == Id);



                var selectedParticipants = attVM.AvailableParticipants.Where(x => x.IsChecked).Select(x => x.Id);
                // Remove existing devices for the customer
                // Note following is for EF6
                dcx.Attendances.RemoveRange(dcx.Attendances.Where(x => x.TrainingId == Id));


                // Add new selections
                foreach (var user in selectedParticipants)
                {
                    Attendance att = new Attendance
                    {
                        ParticipantId = user,
                        IsPresent = true,
                        TrainingId = itr.TrainingId,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Name").Value,
                    };
                    dcx.Attendances.Add(att);
                }
                // Save and redirect
                await dcx.SaveChangesAsync();

            }



            return ViewComponent("ViewAttendances");
        }


        [HttpGet]
        public IActionResult AddParticipant()
        {
            return ViewComponent("AddParticipant");
        }
        [HttpPost]
        public async Task<IActionResult> AddParticipant(AddParticipantVM addParticipantVM)
        {
            Participant addThisParticipant = new Participant()
            {
                Name = addParticipantVM.Name,
                Email = addParticipantVM.Rank,
                Department = addParticipantVM.Department,
                Contact = addParticipantVM.Contact,
            };
            dcx.Participants.Add(addThisParticipant);
            await dcx.SaveChangesAsync();

            return ViewComponent("ViewParticipants");
        }
        [HttpGet]
        public IActionResult ImportFromStaffList()
        {
            return ViewComponent("ImportFromStaffList");
        }
        [HttpPost]
        public async Task<IActionResult> ImportFromStaffList(ImportFromStaffListVM importFromStaffListVM)
        {

            foreach (var staff in importFromStaffListVM.SelectedId)
            {

                var selectedStaff = dcx.StaffList.Where(x => x.StaffId.Contains(staff.ToString())).ToList();

                foreach (var staffid in selectedStaff)
                {
                    Participant newParticipant = new Participant()
                    {
                        DateOfBirth = staffid.DateOfBirth,
                        Contact = staffid.Contact,
                        Name = staffid.Name,
                        Rank = staffid.Rank,
                        Department = staffid.Department,
                    };

                    dcx.Participants.Add(newParticipant);
                    await dcx.SaveChangesAsync();
                }

            };



            return ViewComponent("ViewParticipants");
        }
        [HttpGet]
        public IActionResult ImportFromExcel()
        {
            return ViewComponent("ImportFromExcel");
        }
        [HttpPost]
        public async Task<IActionResult> ImportFromExcel(IFormFile formFile)
        {
            var data = new MemoryStream();
            await formFile.CopyToAsync(data);

            data.Position = 0;
            using (var reader = new StreamReader(data))
            {
                var bad = new List<string>();
                var conf = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    HeaderValidated = null,
                    MissingFieldFound = null,
                    DetectColumnCountChanges = true,
                    SanitizeForInjection = true,

                    BadDataFound = context =>
                    {
                        bad.Add(context.RawRecord);
                    }
                };
                using (var csvReader = new CsvReader(reader, conf))
                {
                    while (csvReader.Read())
                    {
                        var Name = csvReader.GetField(0).ToString();
                        var Contact = csvReader.GetField(1).ToString();
                        var DoB = csvReader.GetField(2);
                        var Department = csvReader.GetField(3).ToString();

                        await dcx.Participants.AddAsync(new Participant
                        {
                            Name = Name.ToString(),
                            Contact = Contact,
                            Department = Department,
                            DateOfBirth = DateTime.Parse(DoB),

                        });
                        dcx.SaveChanges();
                    }
                };
            }
            return ViewComponent("ViewParticipants");
        }
    }
}



