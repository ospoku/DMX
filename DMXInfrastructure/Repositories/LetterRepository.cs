using DMXApplication.Interfaces;
using DMXDomain.Models;
using DMXInfrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMXInfrastructure.Repositories
{
    public class LetterRepository(IEntityRepository ent, XContext dcx) : ILetterRepository
    {

    public void AddLetter(Letter letter)
    {
        ent.Add(letter);

    }
    public void DeleteLetter(Letter letter)
        { 
        ent.Delete(letter);
    }
    public List<Letter> GetLetterList()
    {
        return [.. dcx.Letters];
    }
    public void UpdateLetter(Letter letter)
    {
        ent.Update(letter);
        }
        public async Task<Letter?> GetByIdAsync(Guid id)
        {
            return await dcx.Letters.AsNoTracking().FirstOrDefaultAsync(x=>x.PublicId == id);
        }
        public async Task<List<Letter>> GetAllAsync()
        {
            return await dcx.Letters.AsNoTracking().ToListAsync();
        }
     
    }
}
