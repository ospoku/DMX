using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMX.ViewModels
{
    public class AddTravelRequestVM
    {
        
        public string ReferenceNumber { get; set; }
      
    
        public SelectList TravelTypes { get; set; }
        public string TravelTypeId { get; set; }
        public decimal ConferenceFee { get; set; }
        public DateTime DepartureDate { get; set; }
        public decimal TransportExpenses { get; set; }
        public int NightAbsent { get; set; }
        public DateTime DateofReturn { get; set; }
       
        public string FuelClaim { get; set; }
        public decimal AmountDue { get; set; }
        public string PurposeofJourney { get; set; }
        public string AdditionalNotes { get; set; }
        public SelectList UsersList { get; set; }
        public List<string> SelectedUsers { get; set; }
    }
}
