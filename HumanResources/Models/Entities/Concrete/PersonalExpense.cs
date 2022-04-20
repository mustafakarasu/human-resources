using HumanResources.Models.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace HumanResources.Models.Entities.Concrete
{
    public class PersonalExpense : BaseEntity
    {
        public int PersonalID { get; set; }
        public int CompanyID { get; set; }
        [DisplayName("Harcama Tarihi")]
        public DateTime ExpenseDate { get; set; } // Harcama tarihi
        [DisplayName("Onay Tarihi")]
        public DateTime? ApprovalDate { get; set; }
        [DisplayName("Red Tarihi")]
        public DateTime? RejectionDate { get; set; }
        [DisplayName("Harcama Tutarı")]
        public decimal Amount { get; set; } // Harcama tutarı
        [DisplayName("Harcama Açıklaması")]
        public string Detail { get; set; }
        [DisplayName("Belge")]
        public string ImageUrl { get; set; } // Harcama fişinin görseli
        public int ExpenseID { get; set; }
        public int StatusID { get; set; }
        [DisplayName("Red Sebebi")]
        public string RejectReason { get; set; } // Eğer şirket yöneticisi reddederse reddetme sebebi buraya eklenecek
        public virtual Expense Expense { get; set; }
        public virtual Status Status { get; set; }
        public virtual User User { get; set; }
        public virtual Company Company { get; set; }
    }
}
