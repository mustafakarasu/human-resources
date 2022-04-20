using HumanResources.Models.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanResources.Models.Entities.Concrete
{
    public class PersonalAdvance : BaseEntity
    {
        public int PersonalID { get; set; }
        public int CompanyID { get; set; }
        [DisplayName("Avans Talep Tarihi")]
        public DateTime RequestDate { get; set; }
        [DisplayName("Hesaba Geçiş Tarihi")]
        public DateTime PaymentDate { get; set; } // Ödeme için ileri bir tarih seçilmesi zorunlu
        [DisplayName("Onay Tarihi")]
        public DateTime? ApprovalDate { get; set; }
        [DisplayName("Red Tarihi")]
        public DateTime? RejectionDate { get; set; }
        [DisplayName("Avans Tutarı")]
        public decimal Amount { get; set; } // Avans tutarı maaşı geçemez!
        [DisplayName("Avans Detayı")]
        [Required(ErrorMessage = "Lütfen avans detayını giriniz!")]
        public string Detail { get; set; }
        public int AdvancePaymentID { get; set; }
        public int StatusID { get; set; }
        [DisplayName("Red Sebebi")]
        public string RejectReason { get; set; } // Eğer şirket yöneticisi reddederse reddetme sebebi buraya eklenecek
        public virtual AdvancePayment AdvancePayment { get; set; }
        public virtual Status Status { get; set; }
        public virtual User User { get; set; }
        public virtual Company Company { get; set; }
    }
}
