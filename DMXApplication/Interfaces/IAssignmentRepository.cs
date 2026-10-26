namespace DMXApplication.Interfaces
{
    public interface IAssignmentRepository
    {
        void Add<T>(T model) where T : class;

        Task<int> SaveChangesAsync(string userId);
    }
}