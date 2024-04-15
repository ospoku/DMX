using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMX.ViewModels
{
    public class AddTravelRequestVM
    {
        
        public string ReferenceNumber { get; set; }
        public string Name { get; set; }
        public string RankId { get; set; }
        public SelectList RankList { get; set; }
        public SelectList DeptList { get; set; }
        public string DepartmentId { get; set; }
        public decimal ConferenceFee { get; set; }
        public DateTime DepartureDate { get; set; }
        public decimal TransportExpenses { get; set; }
        public int NightAbsent { get; set; }
        public DateTime DateofReturn { get; set; }
        public int Rate { get; set; }
        public string FuelClaim { get; set; }
        public decimal AmountDue { get; set; }
        public string PurposeofJourney { get; set; }
        public string AdditionalNotes { get; set; }
        public SelectList UsersList { get; set; }
        public List<string> SelectedUsers { get; set; }
    }
}
