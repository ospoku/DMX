
using DMXApplication.Interfaces;

namespace DMXApplication.Services
{
    public class AllowanceService(
        IAllowanceRepository allowanceRepository)
    {
        private readonly IAllowanceRepository allowRep = allowanceRepository;

        public async Task<decimal> PerdiemCalculatorAsync(
            int noOfDays,
            string userId)
        {
            if (noOfDays < 0)
            {
                throw new ArgumentException(
                    "Number of days cannot be negative.",
                    nameof(noOfDays));
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException(
                    "User ID cannot be null or empty.",
                    nameof(userId));
            }

            decimal? perDiem =
                await allowRep.GetPerDiemAmountAsync(userId) ?? throw new InvalidOperationException(
                    $"Per diem not found for user ID: {userId}");
            if (perDiem.Value < 0)
            {
                throw new InvalidOperationException(
                    "Per diem amount cannot be negative.");
            }

            return noOfDays * perDiem.Value;
        }

        public async Task<decimal> TotalAllowanceAsync(
            decimal conferenceFee,
            decimal fuelClaim,
            int noOfDays,
            string userId)
        {
            if (conferenceFee < 0)
            {
                throw new ArgumentException(
                    "Conference fee cannot be negative.",
                    nameof(conferenceFee));
            }

            if (fuelClaim < 0)
            {
                throw new ArgumentException(
                    "Fuel claim cannot be negative.",
                    nameof(fuelClaim));
            }

            decimal perDiemTotal =
                await PerdiemCalculatorAsync(noOfDays, userId);

            return conferenceFee + fuelClaim + perDiemTotal;
        }
    }
}