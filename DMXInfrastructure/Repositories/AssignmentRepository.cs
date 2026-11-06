using DMXApplication.Interfaces;
using DMXInfrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DMXInfrastructure.Repositories
{
    public class AssignmentRepository(XContext context)
        : IAssignmentRepository
    {
        private readonly XContext dcx = context;

     
        public async Task<T?> GetByIdAsync<T>(Guid id) where T : class
        {
            return await dcx.Set<T>().FindAsync(id);
        }
        public async Task<List<T>> GetAllAsync<T>() where T : class
        {
            return await dcx.Set<T>().ToListAsync();
        }
        public async Task<List<T>> GetAllAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return await dcx.Set<T>().Where(predicate).ToListAsync();
        } 
    }
}