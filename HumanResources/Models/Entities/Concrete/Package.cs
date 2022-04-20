using HumanResources.Models.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanResources.Models.Entities.Concrete
{
    public class Package : BaseEntity
    {
        [MaxLength(50)]
        [Required(ErrorMessage ="Lütfen Paket Adı giriniz!")]
        [DisplayName("Paket Adı")]
        public string Name { get; set; }

        [DisplayName("Kişi Sayısı")]
        [Required(ErrorMessage ="Lütfen Kişi Sayısını belirtiniz!")]
        public int NumberOfUser { get; set; }

        [DisplayName("Ücret")]
        [Required(ErrorMessage ="Lütfen Ücreti giriniz!")]
        public string Price { get; set; }

        [DisplayName("Resim")]
        public string ImageURL { get; set; }

        public virtual List<Company> Companies { get; set; }

        public Package()
        {
            Companies = new List<Company>();
        }
    }
}
