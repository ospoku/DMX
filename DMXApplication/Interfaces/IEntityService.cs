using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMXApplication.Interfaces
{
  public  interface IEntityService
    {

        public void Add<T>(T entity) where T : class;
        public void Delete<T>(T entity) where T : class;
        public void Update<T>(T entity) where T : class;
        public Task<bool> Save();
 
    }
}
