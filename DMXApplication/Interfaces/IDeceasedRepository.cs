using DMXApplication.DTO.Deceased;
using DMXDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMXApplication.Interfaces
{
  public  interface IDeceasedRepository
    {
      
        public Task<Deceased?> GetByIdAsync(Guid id);
        public Task<List<Deceased>> GetAllAsync();
        public Task<List<Deceased>> GetByUserIdAsync(string userId);

        Task<List<Deceased>> GetFilteredAsync(DeceasedFilterDTO filter);
      
    }
}
