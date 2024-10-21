using DMX.Data;

namespace DMX.Services
{
    public class FeeService( XContext context)
    {
       public readonly XContext dcx=context;
        public decimal FeeCalculator(int numberOfDays)
        {
            decimal totalFee = 0;

            foreach (var day in dcx.FeeStructures.Select(f => f).ToList())
            {

                if (numberOfDays > day.MaxDays)
                {
                    totalFee += (day.MaxDays - day.MinDays + 1) * day.Fee;
                }
                else if (numberOfDays >= day.MinDays)
                {
                    totalFee += (numberOfDays - day.MinDays + 1) * day.Fee;
                    break;

                }
            }
            return totalFee;
        }
    }
}
