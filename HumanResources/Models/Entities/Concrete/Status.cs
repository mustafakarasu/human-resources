using HumanResources.Models.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace HumanResources.Models.Entities.Concrete
{
    public class Status : BaseEntity
    {
        [DisplayName("Onay Durumu")]
        public string Name { get; set; }
        public virtual List<PersonalPermit> PersonalPermits { get; set; }
        public virtual List<PersonalAdvance> PersonalAdvances { get; set; }
        public virtual List<PersonalExpense> PersonalExpenses { get; set; }

        public Status()
        {
            PersonalPermits = new List<PersonalPermit>();
            PersonalAdvances = new List<PersonalAdvance>();
            PersonalExpenses = new List<PersonalExpense>();
        }
    }
}
