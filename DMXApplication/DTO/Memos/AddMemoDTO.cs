namespace DMXApplication.DTO.Memos;

public class AddMemoDTO
{
    public string Title { get; set; }=string.Empty;

    public string Content { get; set; }=string.Empty;

    public List<string> SelectedUserIds { get; set; } = [];
}
