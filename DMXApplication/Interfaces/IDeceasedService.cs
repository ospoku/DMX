using DMXApplication.DTO.Deceased;
using DMXApplication.DTO.Letters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMXApplication.Interfaces
{
    public interface IDeceasedService
    {
        Task<bool>AddDeceased(AddDeceasedDTO dto);
        Task<bool> DeleteDeceased(DeleteDeceasedDTO dto);
        Task<bool> UpdateDeceased(UpdateDeleteDTO dto);
    }
}
