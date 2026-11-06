
using DMXDomain.Models;

namespace DMXApplication.Interfaces
{
    public interface IMemoRepository
    {
   

     

     

        Task<Memo?> GetByIdAsync(Guid id);

        Task<List<Memo>> GetAllAsync();

  
        void CommentMemo(MemoComment memoComment);
    }
}
