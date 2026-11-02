namespace DMXApplication.DTO.Memos;

public class CommentMemoDTO
{
    public Guid PublicId;
    public string Message;

    public Guid MemoId { get; set; }

    public string Comment { get; set; } = string.Empty;
    public string UserId { get; set; }
}
