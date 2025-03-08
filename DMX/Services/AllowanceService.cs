using DMX.Data;

namespace DMX.Services
{

    public class AllowanceService
    {
        private readonly XContext _context;

        public AllowanceService(XContext context)
        {
            _context = context;
        }

        public decimal PerdiemCalculator(int conferenceFee, int fuel, int noOfDays, int rate)
        {
            decimal totalAllowance = 0;

            totalAllowance = (conferenceFee + fuel) + (noOfDays * rate);

            return totalAllowance;
        }
        public decimal FuelClaim(int transportExpenses, int rate)
        {
            decimal fuelClaim = 0;
            fuelClaim = transportExpenses * rate;
            return fuelClaim;
        }
        public decimal AmountDue(decimal totalAllowance, decimal fuelClaim)
        {
            decimal amountDue = 0;
            amountDue = totalAllowance - fuelClaim;
            return amountDue;
        }
        public decimal TotalAllowance(decimal totalAllowance, decimal fuelClaim)
        {
            decimal total = 0;
            total = totalAllowance + fuelClaim;
            return total;
        }
        public decimal PerdiemRate(string userId)
        {
            decimal rate = 0;
            var rank = _context.PerDiems.Find(userId);
            rate = rank.Amount;
            return rate;
        }
    }
}
