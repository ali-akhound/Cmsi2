namespace AVA.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CompanyPackageName")]
    public partial class CompanyPackageName
    {
        public int ID { get; set; }
        public bool IsRegisterPackage { get; set; }
        public decimal Price { get; set; }
        public bool? Enable { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public virtual ApplicationUser LastModifyUser { get; set; }
        public virtual ApplicationUser CreatorUser { get; set; }
        public virtual ICollection<CompanyPackageNameTranslation> CompanyPackageNameTranslations { get; set; }
    }
}
