using DMXApplication.DTO;

namespace DMXApplication.Interfaces;

public interface IAccountService
{
    Task<AccountOperationResultDTO> LoginAsync(LoginDTO dto);
    Task<AccountOperationResultDTO> UpdateProfileAsync(EditProfileDTO dto);
    Task<AccountOperationResultDTO> DeletePhotoAsync(DeletePhotoDTO dto);
    Task<AccountOperationResultDTO> ConfirmEmailAsync(VerifyEmailDTO dto);
}
