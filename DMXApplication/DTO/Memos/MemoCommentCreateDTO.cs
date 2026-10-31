namespace DMXApplication.DTO.Memos;

public class MemoCommentCreateDTO
{
    public Guid MemoId { get; set; }

    public string Comment { get; set; } = string.Empty;
}
