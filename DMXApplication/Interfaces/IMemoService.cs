using DMXApplication.DTO.Memos;

namespace DMXApplication.Interfaces
{
    public interface IMemoService
    {
        Task<bool> AddMemo(AddMemoDTO dto, string userId);

        Task<bool> DeleteMemo(DeleteMemoDTO dto, string userId);

        Task<bool> UpdateMemo(UpdateMemoDTO dto, string userId);

        Task<bool> CommentMemo(CommentMemoDTO dto, string userId);

        Task<List<MemoDTO>> GetAllAsync();

        Task<MemoDTO?> GetByIdAsync(Guid id);
    }
}