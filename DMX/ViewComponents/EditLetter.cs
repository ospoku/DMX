using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using DMX.Data;
using DMX.Models;
using DMX.ViewModels;
using DMX.DataProtection;
using System.Web;

namespace DMX.ViewComponents
{
    public class EditLetter(XContext dContext, UserManager<AppUser> userManager) : ViewComponent
    {
        public readonly XContext dcx = dContext;
        public readonly UserManager<AppUser> usm = userManager;

        public IViewComponentResult Invoke(string Id)
        {
            // Decode and decrypt PublicId
            var decodedId = HttpUtility.UrlDecode(Id)?.Replace(" ", "+");
            var decryptedId = Encryption.Decrypt(decodedId);
            if (!Guid.TryParse(decryptedId, out Guid letterGuid))
                return View("BadRequest", "Invalid memo ID format.");

            var documentToEdit = dcx.Letters
                .FirstOrDefault(a => a.PublicId == letterGuid && a.IsDeleted == false);

            if (documentToEdit == null)
                return View("NotFound");

            EditLetterVM editLetterVM = new()
            {
                DocumentDate = documentToEdit.DocumentDate,
                AdditionalNotes = documentToEdit.AdditionalNotes,
                Source = documentToEdit.Source,
                ReferenceNumber = documentToEdit.ReferenceNumber,
                DateReceived = documentToEdit.DateReceived,
                Subject = documentToEdit.Subject,

                SelectedUsers = dcx.LetterAssignments
                                   .Where(x => x.PublicId == letterGuid)
                                   .Select(x => x.UserId)
                                   .ToList(),

                UsersList = new SelectList(usm.Users.ToList(), nameof(AppUser.Id), nameof(AppUser.Fullname))
            };

            return View(editLetterVM);
        }
    }
}
