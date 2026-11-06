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
   
        Task<LetterDTO?> GetByIdAsync(Guid id);
        Task<List<LetterDTO>> GetAllAsync();


    }
}
