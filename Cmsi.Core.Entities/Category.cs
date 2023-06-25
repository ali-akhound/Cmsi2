namespace AVA.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Category")]
    public partial class Category
    {
        public Category()
        {
        }
        public int ID { get; set; }

        [StringLength(500)]
        [Required]
        public string Name { get; set; }

        [StringLength(50)]
        [Required]
        public string TableName { get; set; }
        [StringLength(50)]
        public string IconName { get; set; }

        [StringLength(1000)]
        public string ShortImageUrl { get; set; }

        [StringLength(1000)]
        public string LongImageUrl { get; set; }

        [Column(TypeName = "ntext")]
        public string Explain { get; set; }

        [StringLength(1000)]
        public string Summery { get; set; }

        [StringLength(1000)]
        public string PageKeyword { get; set; }

        [StringLength(1000)]
        public string PageDesc { get; set; }

        public bool? AllowDelete { get; set; }

        public int? priority { get; set; }
        public bool? Enable { get; set; }

        public virtual ICollection<CategoryName> CategoryNames { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public virtual ApplicationUser LastModifyUser { get; set; }
        public virtual ApplicationUser CreatorUser { get; set; }
        public virtual Category Parent { get; set; }
        
        //public virtual ICollection<CompanyFeatureName> CompanyFeatureNames { get; set; }

    }
}
