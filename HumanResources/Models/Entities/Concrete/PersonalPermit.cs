using HumanResources.Models.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanResources.Models.Entities.Concrete
{
    public class PersonalPermit : BaseEntity
    {
        public int PersonalID { get; set; }
        public int CompanyID { get; set; }
        [DisplayName("İzin Talep Tarihi")]
        [Required(ErrorMessage = "Lütfen tarih girişi yapınız!")]
        public DateTime RequestDate { get; set; }
        [DisplayName("İzin Başlangıç Tarihi")]
        [Required(ErrorMessage = "Lütfen tarih girişi yapınız!")]
        public DateTime StartDate { get; set; }
        [DisplayName("İzin Bitiş Tarihi")]
        [Required(ErrorMessage = "Lütfen tarih girişi yapınız!")]
        public DateTime EndDate { get; set; }
        [DisplayName("Onay Tarihi")]
        public DateTime? ApprovalDate { get; set; }
        [DisplayName("Red Tarihi")]
        public DateTime? RejectionDate { get; set; }
        [DisplayName("Belge")]
        public string FileUrl { get; set; }
        public int PermissionID { get; set; }
        public int StatusID { get; set; }
        [DisplayName("Red Sebebi")]
        public string RejectReason { get; set; } // Eğer şirket yöneticisi reddederse reddetme sebebi buraya eklenecek
        public virtual Permission Permission { get; set; }
        public virtual Status Status { get; set; }
        public virtual User User { get; set; }
        public virtual Company Company { get; set; }
    }
}
