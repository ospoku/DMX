using DMXApplication.Interfaces;
using DMXDomain.Models;

namespace DMXApplication.Services
{
    public class MemoService(IMemoRepository repository) 
    {
        private readonly IMemoRepository rep = repository;

        public async Task<bool> AddMemoAsync(Memo memo, string userId)
        {
            rep.AddMemo(memo);

            int result = await rep.SaveChangesAsync(userId);

            return result > 0;
        }

        public async Task<bool> DeleteMemoAsync(Guid id, string userId)
        {
            var memo = await rep.GetByIdAsync(id);

            if (memo == null)
                return false;

            rep.DeleteMemo(memo);

            int result = await rep.SaveChangesAsync(userId);

            return result > 0;
        }

        public async Task<bool> UpdateMemoAsync(Memo memo, string userId)
        {
            rep.UpdateMemo(memo);

            int result = await rep  .SaveChangesAsync(userId);

            return result > 0;
        }

        public Task<List<Memo>> GetAllAsync()
        {
            return rep.GetAllAsync();
        }

        public Task<Memo?> GetByIdAsync(Guid id)
        {
            return rep.GetByIdAsync(id);
        }
    }
}