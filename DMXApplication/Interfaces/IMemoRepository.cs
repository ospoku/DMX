
using DMXDomain.Models;

namespace DMXApplication.Interfaces
{
    public interface IMemoRepository
    {
   
        Task<int>AddMemo(Memo memo);

 


        Task<Memo?> GetByIdAsync(Guid id);

        Task<List<Memo>> GetAllAsync();

  
        Task<int> CommentMemo(Comment memoComment);
        Task<int> DeleteMemo(int id);
    }
}
