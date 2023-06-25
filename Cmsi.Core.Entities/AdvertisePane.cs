namespace AVA.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AdvertisePane")]
    public partial class AdvertisePane
    {
        public int ID { get; set; }

        [StringLength(150)]
        public string SlideLink { get; set; }

        [StringLength(150)]
        public string SlidethumbnailImage { get; set; }

        [StringLength(150)]
        public string SlideImage { get; set; }

        [StringLength(200)]
        public string SildeExplain { get; set; }

        [StringLength(10)]
        public string SlideWSize { get; set; }

        [StringLength(10)]
        public string SlideHSize { get; set; }

        public int? SliderPriority { get; set; }

        public DateTime? SlideCreateDate { get; set; }

        public bool? SliderShow { get; set; }

        public byte? Position { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
