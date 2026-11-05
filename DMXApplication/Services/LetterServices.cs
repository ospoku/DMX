using DMXApplication.DTO.Letters;
using DMXApplication.Interfaces;
using DMXDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMXApplication.Services
{
   public  class LetterServices(ILetterRepository rep):ILetterServices
    {
     

        public void AddLetter(AddLetterDTO addLetterDto)
        {
            rep.AddLetter(new Letter
            {
                Source = addLetterDto.Source,
                Subject = addLetterDto.Subject,
                DateReceived = addLetterDto.ReceiptDate,
                DocumentDate = addLetterDto.DocumentDate,
                AdditionalNotes = addLetterDto.AdditionalNotes
            });
        }
        public void DeleteLetter(DeleteLetterDTO deleteLetterDto) 
        { rep.DeleteLetter(new Letter
            {
                PublicId = deleteLetterDto.Id
            });
        }
        public void UpdateLetter(UpdateLetterDTO updateLetterDto)
        {
        rep.UpdateLetter(new Letter
            {
                PublicId = updateLetterDto.Id,
                Source = updateLetterDto.Source,
                Subject = updateLetterDto.Subject,
                DateReceived = updateLetterDto.ReceiptDate,
                DocumentDate = updateLetterDto.DocumentDate,
                AdditionalNotes = updateLetterDto.AdditionalNotes
            });
        }

        public async Task<LetterDTO?> GetByIdAsync(Guid id)
        {
            var letter = await rep.GetByIdAsync(id);
            if (letter == null)
                return null;
            return new LetterDTO
            {
                Id = letter.PublicId,
                Source = letter.Source,
                Subject = letter.Subject,
                ReceiptDate = letter.DateReceived,
                DocumentDate = letter.DocumentDate,
                AdditionalNotes = letter.AdditionalNotes
            };
        }
        public async Task<List<LetterDTO>> GetAllAsync()
        {
            var letters = await rep.GetAllAsync();
            return letters.Select(letter => new LetterDTO
            {
                Id = letter.PublicId,
                Source = letter.Source,
                Subject = letter.Subject,
                ReceiptDate = letter.DateReceived,
                DocumentDate = letter.DocumentDate,
                AdditionalNotes = letter.AdditionalNotes
            }).ToList();
        }
        public async Task<bool> SaveChangesAsync()
        {
            int result = await rep.SaveChangesAsync();
            return result > 0;
        }


    }
}
