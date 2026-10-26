
using DMXApplication.Interfaces;
using DMXDomain.Models;
using DMXInfrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DMXInfrastructure.Repositories
    {
        public class FeeRepository(XContext context) : IFeeRepository
        {
            private readonly XContext _context = context;

        public List<FeeStructure> GetFeeStructures(int deceasedTypeId)
            {
                return _context.FeeStructures
                    .AsNoTracking()
                    .Where(f => f.DeceasedTypeId == deceasedTypeId)
                    .OrderBy(f => f.MinDays)
                    .ToList();
            }
        }
    }



