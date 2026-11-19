using DMXApplication.DTO.Letters;
using DMXApplication.Interfaces;
using DMXDomain.Models;
using System.Threading.Tasks;

namespace DMXApplication.Services
{
   public  class LetterServices(ILetterRepository rep, IEntityService ent):ILetterServices
    {
     


        public async Task<int> AddLetter(AddLetterDTO dto)
        {
            var letter = new Letter
            {
                Source = dto.Source,
                Subject = dto.Subject,
                DateReceived = dto.ReceiptDate,
                DocumentDate = dto.DocumentDate,
                AdditionalNotes = dto.AdditionalNotes
            };
           await rep.AddLetter(letter);
            bool result = await ent.SaveEntity();
            return result ? letter.Id : 0;
        }
        public async Task<bool> DeleteLetter(DeleteLetterDTO deleteLetterDto) 
        { 


             ent.DeleteEntity(new Letter
            {
                PublicId = deleteLetterDto.Id
            });
            return true;
        }
        public async Task<bool> UpdateLetter(UpdateLetterDTO updateLetterDto)
        {
       ent.UpdateEntity(new Letter
            {
                PublicId = updateLetterDto.Id,
                Source = updateLetterDto.Source,
                Subject = updateLetterDto.Subject,
                DateReceived = updateLetterDto.ReceiptDate,
                DocumentDate = updateLetterDto.DocumentDate,
                AdditionalNotes = updateLetterDto.AdditionalNotes
            });
            return true;
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
            return [.. letters.Select(letter => new LetterDTO
            {
                Id = letter.PublicId,
                Source = letter.Source,
                Subject = letter.Subject,
                ReceiptDate = letter.DateReceived,
                DocumentDate = letter.DocumentDate,
                AdditionalNotes = letter.AdditionalNotes
            })];
        }
    


    }
}
