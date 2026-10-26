namespace DMXApplication.DTO;

public sealed class AddDeceasedDTO
{
    public string TagNo { get; set; } = string.Empty;
    public string FolderNo { get; set; } = string.Empty;
    public string WardInCharge { get; set; } = string.Empty;
    public Guid DeceasedTypeId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Depositor { get; set; } = string.Empty;
    public string DepositorAddress { get; set; } = string.Empty;
    public string Diagnoses { get; set; } = string.Empty;
    public string DeceasedName { get; set; } = string.Empty;
    public List<string> SelectedUsers { get; set; } = [];
}
