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
    public class LetterRepository(XContext dcx) : ILetterRepository
    {

    public void AddLetter(Letter letter)
    {
        dcx.Letters.Add(letter);

    }
    public void DeleteLetter(Letter letter)
        { 
        dcx.Letters.Remove(letter);
    }
    public List<Letter> GetLetterList()
    {
        return dcx.Letters.ToList();
    }
    public void UpdateLetter(Letter letter)
    {
        dcx.Letters.Update(letter);
    }
        public async Task<Letter?> GetByIdAsync(Guid id)
        {
            return await dcx.Letters.FindAsync(id);
        }
        public async Task<List<Letter>> GetAllAsync()
        {
            return await dcx.Letters.ToListAsync();
        }
        public async Task<int> SaveChangesAsync(string userId)
        {
            return await  dcx.SaveChangesAsync();
        }
    }
}
