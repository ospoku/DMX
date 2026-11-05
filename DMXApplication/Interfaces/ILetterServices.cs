using DMXApplication.DTO.Letters;
using DMXApplication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMXApplication.Interfaces
{
   public interface ILetterServices
    {
        void AddLetter(AddLetterDTO letter);
        void DeleteLetter(DeleteLetterDTO letter);
        void UpdateLetter(UpdateLetterDTO letter);
        Task<LetterDTO?> GetByIdAsync(Guid id);
        Task<List<LetterDTO>> GetAllAsync();


    }
}
