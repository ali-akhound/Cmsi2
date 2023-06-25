namespace AVA.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CompanyPackageNameTranslation")]
    public partial class CompanyPackageNameTranslation
    {
        public int ID { get; set; }
        [StringLength(200)]
        [Required]
        public string Name { get; set; }
        public virtual CompanyPackageName CompanyPackageName { get; set; }
        public virtual Language Language { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public virtual ApplicationUser LastModifyUser { get; set; }
        public virtual ApplicationUser CreatorUser { get; set; }
    }
}
