namespace DMXApplication.DTO;

public sealed class VerifyEmailDTO
{
    public string UserId { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}
