namespace DMXApplication.DTO.Memos;

public class MemoDTO
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }
}
