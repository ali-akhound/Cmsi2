namespace AVA.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProductGallery")]
    public partial class ProductGallery
    {
        public int ID { get; set; }

        [StringLength(300)]
        public string ShortImageUrl { get; set; }

        [StringLength(300)]
        public string LongImageUrl { get; set; }

        [StringLength(2000)]
        public string Explain { get; set; }

        public int ProductID { get; set; }

        public bool? Enable { get; set; }

        public DateTime? Createdate { get; set; }
    }
}
