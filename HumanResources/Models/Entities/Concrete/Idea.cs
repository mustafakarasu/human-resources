using HumanResources.Models.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanResources.Models.Entities.Concrete
{
    public class Idea : BaseEntity
    {
        [DisplayName("Başlık")]
        [Required(ErrorMessage ="Lütfen Başlığı giriniz!")]
        public string Title { get; set; }

        [DisplayName("Görüş Detayı")]
        [Required(ErrorMessage = "Lütfen Görüşünüzü Detaylarını giriniz!")]
        public string Details { get; set; }

        [DisplayName("İşlem Tarihi")]
        public DateTime ProcessDate { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
