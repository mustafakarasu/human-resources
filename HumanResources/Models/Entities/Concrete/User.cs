using HumanResources.Models.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanResources.Models.Entities.Concrete
{
    public class User : BaseEntity
    {
        [MaxLength(100)]
        [Required(ErrorMessage = "Lütfen Ad alanını giriniz!")]
        [DisplayName("Ad")]
        public string FirstName { get; set; }

        [MaxLength(100)]
        [Required(ErrorMessage = "Lütfen Soyad alanını giriniz!")]
        [DisplayName("Soyad")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Lütfen parolayı giriniz!")]
        [DisplayName("Parola")]
        public string Password { get; set; }
        public bool FirstPasswordEnter { get; set; }

        [DisplayName("Resim")]
        public string ImageURL { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessage = "Lütfen email adresini giriniz!")]
        [EmailAddress]
        public string Email { get; set; }

        [DisplayName("Unvan")]
        public string Title { get; set; }

        [DisplayName("İşe Başlangıç Tarihi")]
        [Required(ErrorMessage = "Lütfen işe başlangıç tarihini giriniz!")]
        public DateTime HireDate { get; set; }

        [DisplayName("İşten Çıkış Tarihi")]
        public DateTime? TerminationDate { get; set; }

        [DisplayName("Birim")]
        public string Unit { get; set; }

        [DisplayName("Doğum Tarihi")]
        public DateTime? BirthDate { get; set; }

        [DisplayName("Mezun Olduğu Üniversite")]
        public string GraduatedFromUniversity { get; set; }

        [DataType("int")]
        [StringLength(11)]
        [DisplayName("T.C. Kimlik No")]
        public string IdentityNumber { get; set; }

        [Required]
        [DisplayName("Cinsiyet")]
        public bool Gender { get; set; }

        [Required]
        [DisplayName("Medeni Durum")]
        public bool MaritalStatus { get; set; }

        [DisplayName("Telefon")]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Bu telefon numarası geçerli değil!")]
        public string Phone { get; set; }

        [DisplayName("Maaş")]
        public decimal? Salary { get; set; }

        public int RoleId { get; set; }

        public virtual Role Role { get; set; }
        public int? CompanyID { get; set; }
        public virtual Company Company { get; set; }
        public virtual List<PersonalPermit> PersonalPermits { get; set; }
        public virtual List<PersonalAdvance> PersonalAdvances { get; set; }
        public virtual List<PersonalExpense> PersonalExpenses { get; set; }

        public User()
        {
            PersonalPermits = new List<PersonalPermit>();
            PersonalAdvances = new List<PersonalAdvance>();
            PersonalExpenses = new List<PersonalExpense>();
        }
    }
}
