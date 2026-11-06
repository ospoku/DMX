using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMXApplication.Interfaces
{
  public  interface IEntityService
    {
        
        public Task<bool> Add<T>(T entity) where T : class;
        public Task<bool> Delete<T>(T entity) where T : class;
        public Task<bool> Update<T>(T model) where T : class;
    }
}
