using DMXDomain.Models;

namespace DMXApplication.Interfaces
{
    public interface ILetterRepository
    {
     
        Task<Letter?> GetByIdAsync(Guid id);
        Task<List<Letter>> GetAllAsync();
     
    }
}
