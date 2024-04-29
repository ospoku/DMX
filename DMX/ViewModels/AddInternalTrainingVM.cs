
namespace DMX.ViewModels
{
    public class AddInternalTrainingVM
    {
       
        public string WorkshopTitle { get; internal set; }
        public int NumberofDays { get; internal set; }
        
        public DateTime TrainingDate { get; internal set; }
        public string TrainingGroup { get; internal set; }
        public string Description { get; internal set; }
    }
}
