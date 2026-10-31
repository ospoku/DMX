
using DMXDomain.Models;

namespace DMXApplication.Interfaces
{
    public interface IMemoRepository
    {
        void AddMemo(Memo memo);

        void UpdateMemo(Memo memo);

        void DeleteMemo(Memo memo);

        Task<Memo?> GetByIdAsync(Guid id);

        Task<List<Memo>> GetAllAsync();

        Task<int> SaveChangesAsync(string userId);
    }
}
