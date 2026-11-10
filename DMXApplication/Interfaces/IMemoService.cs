using DMXApplication.DTO.Memos;

namespace DMXApplication.Interfaces
{
    public interface IMemoService
    {

        Task<int> AddMemo(AddMemoDTO dto); 
        Task<bool> CommentMemo(CommentMemoDTO dto);

        Task<List<MemoDTO>> GetAllAsync();

        Task<MemoDTO?> GetByIdAsync(Guid id);
    }
}