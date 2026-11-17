
using DMXApplication.Interfaces;



namespace DMXApplication.Services
{

    public class EntityService(ICurrentUserService currentUser,
        IEntityRepository persistence) : IEntityService
    {

        private readonly IEntityRepository entPer = persistence;

        public void AddEntity<T>(
            T entity) where T : class
        {
          
                var entityType = typeof(T);

                entityType.GetProperty("CreatedBy")
                    ?.SetValue(entity, currentUser.UserId);

                entityType.GetProperty("CreatedDate")
                    ?.SetValue(entity, DateTime.UtcNow);



                entPer.Add(entity);

      
        }

        public void UpdateEntity<T>(T entity) where T : class
        {
            
                var entityType = typeof(T);

                entityType.GetProperty("UpdatedBy")
                    ?.SetValue(entity, currentUser.UserId);

                entityType.GetProperty("UpdatedDate")
                    ?.SetValue(entity, DateTime.UtcNow);

            

        }

        public void DeleteEntity<T>(T entity) where T : class
        {
          
                var entityType = typeof(T);
                entityType.GetProperty("DeletedBy")?.SetValue(entity, currentUser.UserId);
                entityType.GetProperty("IsDeleted")?.SetValue(entity, true);
                entityType.GetProperty("DeleteDate")?.SetValue(entity, DateTime.UtcNow);
          

        
        }
        public async Task<bool> SaveEntity()
        {
            try
            {
                await entPer.SaveChangesAsync(currentUser.UserId);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


    }
}
