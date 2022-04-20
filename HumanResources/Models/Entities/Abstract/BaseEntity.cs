using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace HumanResources.Models.Entities.Abstract
{
    public class BaseEntity
    {
        public int Id { get; set; }
        [DisplayName("Oluşturulma Tarihi")]
        public DateTime CreatedDate { get; set; }
        [DisplayName("Güncellenme Tarihi")]
        public DateTime? ModifiedDate { get; set; }
        [DisplayName("Silinme Tarihi")]
        public DateTime? DeletedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
