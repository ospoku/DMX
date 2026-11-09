using System.Linq.Expressions;

namespace DMXApplication.Interfaces
{
    public interface IAssignmentRepository
    {
        public Task<T>AddAssignee<T>() where T : class;
       public Task<T?> GetByIdAsync<T>(Guid id) where T : class;
        public Task<List<T>> GetAllAsync<T>() where T : class;
        public Task<List<T>> GetAllAsync<T>(Expression<Func<T, bool>> predicate) where T : class;
    }
}