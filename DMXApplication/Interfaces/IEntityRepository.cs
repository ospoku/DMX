namespace DMXApplication.Interfaces
    {
        public interface IEntityRepository
    {
         public  Task Add<T>(T entity) where T : class;

         public Task Update<T>(T entity) where T : class;
        public Task  Delete<T>(T entity) where T : class;
        public Task<int> SaveChangesAsync(string userId);
        }
    }



