namespace AVA.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProjectTable")]
    public partial class ProjectTable
    {
        public int ID { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        [Column(TypeName = "ntext")]
        public string Explain { get; set; }

        public int? CatID { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(300)]
        public string ShortImageUrl { get; set; }

        [StringLength(300)]
        public string LongImageUrl { get; set; }

        [StringLength(4000)]
        public string ExplainSummery { get; set; }

        public int? Position { get; set; }

        [StringLength(500)]
        public string PageKeyword { get; set; }

        [StringLength(500)]
        public string PageDesc { get; set; }

        [StringLength(500)]
        public string SourceUrl { get; set; }

        public bool? PublicAccess { get; set; }

        public bool? Enable { get; set; }

        public int? Visit { get; set; }

        public virtual Category Category { get; set; }
    }
}
