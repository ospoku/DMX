
using DMXApplication.Interfaces;



namespace DMXApplication.Services
{
   
        public class EntityService( ICurrentUserService currentUser,
            IEntityRepository persistence):IEntityService
        {
       
            private readonly IEntityRepository entPer = persistence;

            public async Task<bool> Add<T>(
                T entity) where T : class
            {
                try
                {
          
                    var entityType = typeof(T);

                    entityType.GetProperty("CreatedBy")
                        ?.SetValue(entity, currentUser.UserId);

                    entityType.GetProperty("CreatedDate")
                        ?.SetValue(entity, DateTime.UtcNow);

                    entityType.GetProperty("IsDeleted")
                        ?.SetValue(entity, false);

                    entPer.Add(entity);

                    await entPer.SaveChangesAsync(currentUser.UserId);

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public async Task<bool> Update<T>(
                T model) where T : class
            {
                try
                {
               

                    var entityType = typeof(T);

                    entityType.GetProperty("ModifiedBy")
                        ?.SetValue(model, currentUser.UserId);

                    entityType.GetProperty("ModifiedDate")
                        ?.SetValue(model, DateTime.UtcNow);

                    entPer.Update(model);

                    await entPer.SaveChangesAsync(currentUser.UserId);

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public async Task<bool> Delete<T>(
                T model) where T : class
            {
                try
                {
              
                    var entityType = typeof(T);

                    entityType.GetProperty("ModifiedBy")
                        ?.SetValue(model, currentUser.UserId);

                    entityType.GetProperty("ModifiedDate")
                        ?.SetValue(model, DateTime.UtcNow);

                    entityType.GetProperty("IsDeleted")
                        ?.SetValue(model, true);

                    entPer.Update(model);

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
