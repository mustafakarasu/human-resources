using HumanResources.Models.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace HumanResources.Models.Entities.Concrete
{
    public class Expense : BaseEntity
    {
        [DisplayName("Harcama Sebebi")]
        public string Name { get; set; }
        public virtual List<PersonalExpense> PersonalExpenses { get; set; }
        public Expense()
        {
            PersonalExpenses = new List<PersonalExpense>();
        }
    }
}
