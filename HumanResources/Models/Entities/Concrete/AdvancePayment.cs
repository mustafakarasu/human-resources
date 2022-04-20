using HumanResources.Models.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace HumanResources.Models.Entities.Concrete
{
    public class AdvancePayment : BaseEntity
    {
        [DisplayName("Avans Sebebi")]
        public string Name { get; set; }
        public virtual List<PersonalAdvance> PersonalAdvances { get; set; }
        public AdvancePayment()
        {
            PersonalAdvances = new List<PersonalAdvance>();
        }
    }
}
