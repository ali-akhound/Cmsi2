namespace AVA.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ImageGallery")]
    public partial class ImageGallery
    {
        public int ID { get; set; }

        [StringLength(200)]
        public string LongImageUrl { get; set; }

        [StringLength(200)]
        public string ShortImageUrl { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Explain { get; set; }

        public bool? Visible { get; set; }

        public int? Position { get; set; }

        [StringLength(500)]
        public string ImageLink { get; set; }

        public int? Height { get; set; }

        public int? Width { get; set; }

        public int? FK_GroupID { get; set; }

        public bool? Enable { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public virtual ApplicationUser LastModifyUser { get; set; }
        public virtual ApplicationUser CreatorUser { get; set; }
    }
}
