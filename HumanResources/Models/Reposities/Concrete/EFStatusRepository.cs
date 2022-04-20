using HumanResources.Models.Entities.Concrete;
using HumanResources.Models.Entities.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanResources.Models.Reposities.Concrete
{
    public class EFStatusRepository : EFRepositoryBase<Status>
    {
        private ProjectContext _context;
        public EFStatusRepository(ProjectContext context) : base(context)
        {
            _context = context;
        }
    }
}
