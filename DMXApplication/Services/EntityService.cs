
using DMXApplication.Interfaces;



namespace DMXApplication.Services
{

    public class EntityService(ICurrentUserService currentUser,
        IEntityRepository persistence) : IEntityService
    {

        private readonly IEntityRepository entPer = persistence;

        public async Task<bool> Save<T>(
            T entity) where T : class
        {
            try
            {

                var entityType = typeof(T);

                entityType.GetProperty("CreatedBy")
                    ?.SetValue(entity, currentUser.UserId);

                entityType.GetProperty("CreatedDate")
                    ?.SetValue(entity, DateTime.UtcNow);



                entPer.Add(entity);

                await entPer.SaveChangesAsync(currentUser.UserId);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Update<T>(T entity) where T : class
        {
            try
            {
                var entityType = typeof(T);

                entityType.GetProperty("UpdatedBy")
                    ?.SetValue(entity, currentUser.UserId);

                entityType.GetProperty("UpdatedDate")
                    ?.SetValue(entity, DateTime.UtcNow);

                await entPer.SaveChangesAsync(currentUser.UserId);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Delete<T>(T entity) where T : class
        {
            try
            {
                var entityType = typeof(T);
                entityType.GetProperty("DeletedBy")?.SetValue(entity, currentUser.UserId);
                entityType.GetProperty("IsDeleted")?.SetValue(entity, true);
                entityType.GetProperty("DeleteDate")?.SetValue(entity, DateTime.UtcNow);
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
