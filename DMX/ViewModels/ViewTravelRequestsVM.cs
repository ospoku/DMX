namespace DMX.ViewModels
{
    public class ViewTravelRequestsVM
    {
        public string TravelRequestId { get; set; }

        public string ReferenceNumber { get; set; }
        public string Name { get; set; }
        public string RankId { get; set; }
        public string DepartmentId { get; set; }
        public decimal ConferenceFee { get; set; }
        public DateTime DepartureDate { get; set; }
        public string TravelType { get; set; }
        public decimal TransportExpenses { get; set; }
        public int NightAbsent { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime DateofReturn { get; set; }
        public DateTime StartDate { get; set; }
        public int TotalAllowance { get; set; }
        public int Rate { get; set; }
        public string FuelClaim { get; set; }
        public decimal AmountDue { get; set; }
        public string PurposeofJourney { get; set; }
        public DateTime? CreatedDate { get; internal set; }
    }
}
