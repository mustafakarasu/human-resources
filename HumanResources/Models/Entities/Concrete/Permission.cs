using HumanResources.Models.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace HumanResources.Models.Entities.Concrete
{
    public class Permission : BaseEntity
    {
        [DisplayName("İzin Sebebi")]
        public string Name { get; set; }
        public bool DocumentRequired { get; set; } // Eğer true ise View'da geri tarihli giriş yapılacak ve belge yüklenmesi istenecek. False ise ileri tarihli giriş yapılacak ve belge istenmeyecek
        public virtual List<PersonalPermit> PersonalPermits { get; set; }
        public Permission()
        {
            PersonalPermits = new List<PersonalPermit>();
        }
    }
}
