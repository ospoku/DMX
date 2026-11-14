using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMXApplication.Interfaces
{
  public  interface IEntityService
    {
        
        public Task<bool> Save<T>(T entity) where T : class;
 
    }
}
