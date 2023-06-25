namespace AVA.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("News")]
    public partial class News
    {
        public int ID { get; set; }

        [StringLength(250)]
        public string Subject { get; set; }

        [StringLength(800)]
        public string Summery { get; set; }

        [Column(TypeName = "ntext")]
        public string Explain { get; set; }

        [StringLength(200)]
        public string LongImageUrl { get; set; }
        [StringLength(200)]
        public string ShortImageUrl { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime Date { get; set; }

        [StringLength(500)]
        public string Keywords { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public bool? Enable { get; set; }

        public int? Position { get; set; }

        public bool? IsEspecial { get; set; }

        public byte? Type { get; set; }

        public int? Visit { get; set; }
        public virtual Language Language { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public virtual ApplicationUser LastModifyUser { get; set; }
        public virtual ApplicationUser CreatorUser { get; set; }
    }
}
