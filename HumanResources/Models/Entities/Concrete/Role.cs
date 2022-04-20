using HumanResources.Models.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanResources.Models.Entities.Concrete
{
    public class Role : BaseEntity
    {
        [Required(ErrorMessage ="Lütfen Rol Adını giriniz!")]

        [DisplayName("Rol Adı")]
        public string Name { get; set; }


        [DisplayName("Açıklama")]
        public string Description { get; set; }

        public virtual List<User> Users { get; set; }

        public Role()
        {
            Users = new List<User>();
        }

    }
}
