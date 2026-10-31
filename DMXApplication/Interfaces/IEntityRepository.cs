namespace DMXApplication.Interfaces
    {
        public interface IEntityRepository
    {
         public  void Add<T>(T entity) where T : class;

         public void Update<T>(T entity) where T : class;
        public void  Delete<T>(T entity) where T : class;
        public Task<int> SaveChangesAsync(string userId);
        }
    }



