using DMX.Data;

namespace DMX.Services
{
    public class FeeService
    {
        private readonly XContext dcx;

        public FeeService(XContext context)
        {
            dcx = context;
        }

        public decimal FeeCalculator(int numberOfDays)
        {
            decimal totalFee = 0;

            // Get the fee structures in ascending order of MinDays
            var feeStructures = dcx.FeeStructures.OrderBy(f => f.MinDays).ToList();

            foreach (var tier in feeStructures)
            {
                if (numberOfDays > tier.MaxDays)
                {
                    // Calculate fee for the entire tier
                    totalFee += (tier.MaxDays - tier.MinDays + 1) * tier.Fee;
                }
                else if (numberOfDays >= tier.MinDays)
                {
                    // Calculate fee for the remaining days within the tier
                    totalFee += (numberOfDays - tier.MinDays + 1) * tier.Fee;
                    return totalFee;
                }
            }

            // Handle days beyond the last tier
            var lastTier = feeStructures.LastOrDefault();
            if (lastTier != null && numberOfDays > lastTier.MaxDays)
            {
                totalFee += (numberOfDays - lastTier.MaxDays) * lastTier.Fee;
            }

            return totalFee;
        }
    }
}
