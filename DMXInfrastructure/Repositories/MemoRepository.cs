
using DMXApplication.Interfaces;
using DMXDomain.Models;
using DMXInfrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DMXInfrastructure.Repositories
{
    public class MemoRepository(XContext context) : IMemoRepository
    {
        private readonly XContext dcx = context;



        public async Task<Memo?> GetByIdAsync(Guid id)
        {
            return await dcx.Memos
                .FirstOrDefaultAsync(m => m.PublicId == id);
        }

        public async Task<List<Memo>> GetAllAsync()
        {
            return await dcx.Memos.ToListAsync();
        }

     
        public void CommentMemo(MemoComment memoComment)
        {
            dcx.MemoComments.Add(memoComment);
        }
    }
}
