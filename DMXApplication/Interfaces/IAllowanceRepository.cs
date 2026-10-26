namespace DMXApplication.Interfaces
{
 
        public interface IAllowanceRepository
        {
            Task<decimal?> GetPerDiemAmountAsync(string userId);
        }
    }

