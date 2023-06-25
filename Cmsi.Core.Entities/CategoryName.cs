namespace AVA.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CategoryName")]
    public partial class CategoryName
    {
        public CategoryName()
        {
        }
        public int ID { get; set; }

        [StringLength(500)]
        [Required]
        public string Name { get; set; }
        [Column(TypeName = "ntext")]
        public string Explain { get; set; }

        public virtual Language Language { get; set; }
        public virtual Category Category { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public virtual ApplicationUser LastModifyUser { get; set; }
        public virtual ApplicationUser CreatorUser { get; set; }
        
        //public virtual ICollection<CompanyFeatureName> CompanyFeatureNames { get; set; }

    }
}
