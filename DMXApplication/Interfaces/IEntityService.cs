using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMXApplication.Interfaces
{
  public  interface IEntityService
    {

        public void AddEntity<T>(T entity) where T : class;
        public void DeleteEntity<T>(T entity) where T : class;
        public void UpdateEntity<T>(T entity) where T : class;
        public Task<bool> SaveEntity();
 
    }
}
