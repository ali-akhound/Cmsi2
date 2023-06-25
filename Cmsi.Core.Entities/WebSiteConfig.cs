namespace AVA.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WebSiteConfig")]
    public partial class WebSiteConfig
    {
        public int ID { get; set; }

        [StringLength(500)]
        public string LogoUrl { get; set; }

        [StringLength(50)]
        public string BodyForeColor { get; set; }

        public int? LogoWidth { get; set; }

        public int? LogoHeight { get; set; }

        public int? FontSize { get; set; }

        [StringLength(50)]
        public string SmsUser { get; set; }

        [StringLength(50)]
        public string SmsPassword { get; set; }
    }
}
