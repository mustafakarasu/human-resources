using HumanResources.Models.Entities.Concrete;
using HumanResources.Models.Entities.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanResources.Models.Reposities.Concrete
{
    public class EFPersonalExpenseRepository : EFRepositoryBase<PersonalExpense>
    {
        private ProjectContext _context;
        public EFPersonalExpenseRepository(ProjectContext context) : base(context)
        {
            _context = context;
        }
    }
}
