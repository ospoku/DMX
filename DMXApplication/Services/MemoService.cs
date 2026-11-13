using DMXApplication.DTO.Memos;
using DMXApplication.Interfaces;
using DMXDomain.Models;

namespace DMXApplication.Services;

public class MemoService(IMemoRepository rep, IEntityService ent) : IMemoService
{


    public async Task<int> AddMemo(AddMemoDTO dto)
    {
        var memo = new Memo
        {
            Title = dto.Title,
            Content = dto.Content
        };

        await rep.AddMemo(memo);


  

        bool result = await ent.sa;

        return result ? memo.Id : 0;
    
    }
    public async Task<List<MemoDTO>> GetAllAsync()
    {
        var memos = await rep.GetAllAsync();

        return [.. memos.Select(memo => new MemoDTO
        {
            Id = memo.PublicId,
            Title = memo.Title,
            Content = memo.Content
        })];
    }

    public async Task<MemoDTO?> GetByIdAsync(Guid id)
    {
        var memo = await rep.GetByIdAsync(id);

        if (memo == null)
            return null;

        return new MemoDTO
        {
            Id = memo.PublicId,
            Title = memo.Title,
            Content = memo.Content
        };
    }

 
    public async Task<bool> CommentMemo(CommentMemoDTO dto)
    {
        var memo = await rep.GetByIdAsync(dto.PublicId);
        if (memo == null)
            return false;
        var memoComment = new Comment
        {
            MemoId = memo.Id,
            Message = dto.Message
        };
        return await ent.Add(memoComment);
    }
}