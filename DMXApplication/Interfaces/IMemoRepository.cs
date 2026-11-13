
using DMXDomain.Models;

namespace DMXApplication.Interfaces
{
    public interface IMemoRepository
    {
   
        Task<int>AddMemo(Memo memo);
     

     Task<int>DeleteMemo(Memo memo);

        Task<Memo?> GetByIdAsync(Guid id);

        Task<List<Memo>> GetAllAsync();

  
        void CommentMemo(Comment memoComment);
    }
}
