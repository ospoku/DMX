using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DMXApplication.Interfaces
    {
        public interface IEntityPersistence
    {
            void Add<T>(T entity) where T : class;

            void Update<T>(T entity) where T : class;

            Task<int> SaveChangesAsync(string userId);
        }
    }



