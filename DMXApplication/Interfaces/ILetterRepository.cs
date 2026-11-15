using DMXDomain.Models;

namespace DMXApplication.Interfaces
{
    public interface ILetterRepository
    {
     Task<int>AddLetter(Letter letter);
        Task<int>DeleteLetter(Letter letter);
        Task<int>UpdateLetter(Letter letter);
        Task<Letter?> GetByIdAsync(Guid id);
        Task<List<Letter>> GetAllAsync();
     
    }
}
