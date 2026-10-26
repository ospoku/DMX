namespace DMXApplication.DTO;

public sealed class AccountOperationResultDTO
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}
