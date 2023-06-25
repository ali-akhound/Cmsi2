namespace AVA.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GalleryAlbum")]
    public partial class GalleryAlbum
    {
        public int ID { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? Enable { get; set; }
        public virtual Language Language { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public virtual ICollection<GalleryImage> GalleryImages { get; set; }
        public virtual ApplicationUser LastModifyUser { get; set; }
        public virtual ApplicationUser CreatorUser { get; set; }
    }
}
