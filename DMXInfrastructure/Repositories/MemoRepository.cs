
using DMXApplication.Interfaces;
using DMXDomain.Models;
using DMXInfrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DMXInfrastructure.Repositories
{
    public class MemoRepository(XContext context) : IMemoRepository
    {
        private readonly XContext dcx = context;

        public void AddMemo(Memo memo)
        {
            dcx.Memos.Add(memo);
        }

        public void UpdateMemo(Memo memo)
        {
            dcx.Memos.Update(memo);
        }

        public void DeleteMemo(Memo memo)
        {
            dcx.Memos.Remove(memo);
        }

        public async Task<Memo?> GetByIdAsync(Guid id)
        {
            return await dcx.Memos
                .FirstOrDefaultAsync(m => m.PublicId == id);
        }

        public async Task<List<Memo>> GetAllAsync()
        {
            return await dcx.Memos.ToListAsync();
        }

        public Task<int> SaveChangesAsync(string userId)
        {
            return dcx.SaveChangesAsync(userId);
        }
    }
}
