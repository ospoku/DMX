namespace DMX.Models
{
    
        public class TierCharge
        {
            public int MinDays { get; set; }
            public int MaxDays { get; set; }
            public int DaysCharged { get; set; }
            public decimal Fee { get; set; }
            public decimal TotalCharge { get; set; }
        }

    }

