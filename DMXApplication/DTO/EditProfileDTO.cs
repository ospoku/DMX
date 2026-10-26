namespace DMXApplication.DTO;

public sealed class EditProfileDTO
{
    public string UserId { get; set; } = string.Empty;
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string? Telephone { get; set; }
    public byte[]? Picture { get; set; }
}
