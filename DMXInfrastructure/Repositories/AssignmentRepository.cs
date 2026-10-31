using DMXApplication.Interfaces;
using DMXInfrastructure.Data;

namespace DMXInfrastructure.Repositories
{
    public class AssignmentRepository(XContext context)
        : IAssignmentRepository
    {
        private readonly XContext dcx = context;

        public void Add<T>(T model) where T : class
        {
           dcx.Set<T>().Add(model);
        }

        public Task<int> SaveChangesAsync(string userId)
        {
            return dcx.SaveChangesAsync(userId);
        }
    }
}