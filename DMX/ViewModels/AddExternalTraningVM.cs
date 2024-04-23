
namespace DMX.ViewModels
{
    public class AddInternalTraningVM
    {
        public string StaffId { get; internal set; }
        public string WorkshopTitle { get; internal set; }
        public int NumberofDays { get; internal set; }
        public DateTime DepartureDate { get; internal set; }
        public DateTime ReturnDate { get; internal set; }
        public DateTime ProposedTrainingDate { get; internal set; }
        public string ProposedTrainingGroup { get; internal set; }
        public string Description { get; internal set; }
    }
}
