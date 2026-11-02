namespace DMXApplication.DTO.Memos;

public class UpdateMemoDTO
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public List<string> SelectedUserIds { get; set; } = new();
    public Guid PublicId { get; set; }
}
