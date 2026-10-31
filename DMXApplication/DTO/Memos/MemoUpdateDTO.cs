namespace DMXApplication.DTO.Memos;

public class MemoUpdateDTO
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public List<string> SelectedUserIds { get; set; } = new();
}
