using DMXApplication.DTO.Letters;
using DMXApplication.Interfaces;
using DMXDomain.Models;
using System.Threading.Tasks;

namespace DMXApplication.Services
{
   public  class LetterServices(ILetterRepository rep, IEntityService ent):ILetterServices
    {
     


        public async Task<bool> AddLetter(AddLetterDTO dto)
        {
            var letter = new Letter
            {
                Source = dto.Source,
                Subject = dto.Subject,
                DateReceived = dto.ReceiptDate,
                DocumentDate = dto.DocumentDate,
                AdditionalNotes = dto.AdditionalNotes
            };

            return await ent.Add(letter);
        }
        public async Task<bool> DeleteLetter(DeleteLetterDTO deleteLetterDto) 
        { 


            return await ent.Delete(new Letter
            {
                PublicId = deleteLetterDto.Id
            });
        }
        public async Task<bool> UpdateLetter(UpdateLetterDTO updateLetterDto)
        {
        return await ent.Update(new Letter
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
    


    }
}
