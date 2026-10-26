using DMXApplication.Interfaces;
using DMXInfrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DMXInfrastructure.Repositories
{
    public class AllowanceRepository : IAllowanceRepository
    {
        private readonly XContext _context;

        public AllowanceRepository(XContext context)
        {
            _context = context;
        }

        public async Task<decimal?> GetPerDiemAmountAsync(string userId)
        {
            return await _context.PerDiems
                .Where(p => p.AppUser.Id == userId)
                .Select(p => (decimal?)p.Amount)
                .FirstOrDefaultAsync();
        }
    }
}