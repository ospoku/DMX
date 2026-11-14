
using DMXApplication.Interfaces;
using DMXDomain.Models;
using DMXInfrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DMXInfrastructure.Repositories
{
    public class MemoRepository(XContext context) : IMemoRepository
    {
        private readonly XContext dcx = context;

        public async Task<int> AddMemo(Memo memo)
        {
          await  dcx.Memos.AddAsync(memo);
            return  memo.Id;
        }
        public async Task<int>DeleteMemo(int id)
        {
                var memo = await dcx.Memos
                    .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);

            if (memo == null )
                return 0;

                memo.IsDeleted = true;
                

                return memo.Id;
            
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

     
        public async Task<int> CommentMemo(Comment comment)
        {
          await dcx.Comments.AddAsync(comment);
            return comment.Id;
        }
    }
}
