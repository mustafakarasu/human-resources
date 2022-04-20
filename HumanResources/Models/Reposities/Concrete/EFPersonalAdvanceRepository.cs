using HumanResources.Models.Entities.Concrete;
using HumanResources.Models.Entities.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanResources.Models.Reposities.Concrete
{
    public class EFPersonalAdvanceRepository : EFRepositoryBase<PersonalAdvance>
    {
        private ProjectContext _context;
        public EFPersonalAdvanceRepository(ProjectContext context) : base(context)
        {
            _context = context;
        }
    }
}
