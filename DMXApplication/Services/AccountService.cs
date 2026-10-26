using DMXApplication.DTO;
using DMXApplication.Interfaces;
using DMXDomain.Models;
using Microsoft.AspNetCore.Identity;

namespace DMXApplication.Services;

/// <summary>
/// Starter application service for account operations.
/// Ensure DMXApplication references the ASP.NET Core Identity abstractions
/// used by your project before registering this implementation.
/// </summary>
public sealed class AccountService(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager) : IAccountService
{
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly SignInManager<AppUser> _signInManager = signInManager;

    public async Task<AccountOperationResultDTO> LoginAsync(LoginDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Username) ||
            string.IsNullOrWhiteSpace(dto.Password))
        {
            return Failure("Username and password are required.");
        }

        var result = await _signInManager.PasswordSignInAsync(
            dto.Username, dto.Password, dto.RememberMe, lockoutOnFailure: false);

        return result.Succeeded
            ? Success("Login successful.")
            : Failure("Invalid username or password.");
    }

    public async Task<AccountOperationResultDTO> UpdateProfileAsync(EditProfileDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.UserId))
            return Failure("A valid user ID is required.");

        var user = await _userManager.FindByIdAsync(dto.UserId);
        if (user is null)
            return Failure("User not found.");

        user.Firstname = dto.Firstname;
        user.Lastname = dto.Lastname;
        user.PhoneNumber = dto.Telephone;

        if (dto.Picture is not null)
            user.Picture = dto.Picture;

        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded
            ? Success("Profile successfully updated.")
            : Failure(string.Join(" ", result.Errors.Select(e => e.Description)));
    }

    public async Task<AccountOperationResultDTO> DeletePhotoAsync(DeletePhotoDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.UserId))
            return Failure("A valid user ID is required.");

        var user = await _userManager.FindByIdAsync(dto.UserId);
        if (user is null)
            return Failure("User not found.");

        user.Picture = null;
        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded
            ? Success("Photo successfully deleted.")
            : Failure(string.Join(" ", result.Errors.Select(e => e.Description)));
    }

    public async Task<AccountOperationResultDTO> ConfirmEmailAsync(VerifyEmailDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.UserId) ||
            string.IsNullOrWhiteSpace(dto.Token))
        {
            return Failure("Invalid verification link.");
        }

        var user = await _userManager.FindByIdAsync(dto.UserId);
        if (user is null)
            return Failure("User not found.");

        var result = await _userManager.ConfirmEmailAsync(user, dto.Token);

        return result.Succeeded
            ? Success("Email successfully verified. You can now log in.")
            : Failure("Email verification failed.");
    }

    private static AccountOperationResultDTO Success(string message) =>
        new() { Success = true, Message = message };

    private static AccountOperationResultDTO Failure(string message) =>
        new() { Success = false, Message = message };
}
