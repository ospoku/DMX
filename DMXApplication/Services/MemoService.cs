using DMXApplication.DTO.Memos;
using DMXApplication.Interfaces;
using DMXDomain.Models;

namespace DMXApplication.Services;

public class MemoService(IMemoRepository rep, IEntityService ent) : IMemoService
{
    public async Task<bool> AddMemo(
        AddMemoDTO dto)
    {
        var memo = new Memo
        {
            Title = dto.Title,
            Content = dto.Content
        };

    return     await ent.Add(memo);

     

     
    }

    public async Task<bool> DeleteMemo(DeleteMemoDTO dto)
    {
        var memo = await rep.GetByIdAsync(dto.PublicId);

        if (memo == null)
            return false;

     return   await ent.Delete(memo);


  
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

    public async Task<bool> UpdateMemo(UpdateMemoDTO dto)
    {
        var memo = await rep.GetByIdAsync(dto.PublicId);
        if (memo == null)
            return false;
        memo.Title = dto.Title;
        memo.Content = dto.Content;
     return await   ent.Update(memo);
      
    }
    public async Task<bool> CommentMemo(CommentMemoDTO dto)
    {
        var memo = await rep.GetByIdAsync(dto.PublicId);
        if (memo == null)
            return false;
        var memoComment = new MemoComment
        {
            MemoId = memo.Id,
            Message = dto.Message
        };
        return await ent.Add(memoComment);
    }
}