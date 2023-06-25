namespace AVA.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Slider")]
    public partial class Slider
    {
        public int ID { get; set; }

        [StringLength(150)]
        public string Link { get; set; }

        [StringLength(150)]
        public string ThumbnailImage { get; set; }

        [StringLength(150)]
        public string Image { get; set; }

        [Column(TypeName = "ntext")]
        public string Context { get; set; }

        [StringLength(10)]
        public string Width { get; set; }

        [StringLength(10)]
        public string Height { get; set; }

        public int? Priority { get; set; }

        public DateTime? CreateDate { get; set; }

        public bool Enable { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public virtual ApplicationUser LastModifyUser { get; set; }
        public virtual ApplicationUser CreatorUser { get; set; }
    }
}
