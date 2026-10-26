using DMXDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMXApplication.Interfaces
{
  public interface IFeeRepository
    {

        public List<FeeStructure> GetFeeStructures(int deceasedTypeId);
    }
}
