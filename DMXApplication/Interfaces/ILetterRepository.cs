using DMXDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMXApplication.Interfaces
{
    public interface ILetterRepository
    {
        void AddLetter(Letter letter);
        void DeleteLetter(Letter letter);
        void UpdateLetter(Letter letter);
        Task<Letter?> GetByIdAsync(Guid id);
        Task<List<Letter>> GetAllAsync();
        Task<int> SaveChangesAsync();
    }
}
