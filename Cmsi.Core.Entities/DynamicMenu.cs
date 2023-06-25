namespace AVA.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DynamicMenu")]
    public partial class DynamicMenu
    {
        public long ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        public int? ParentID { get; set; }

        public bool? Enable { get; set; }

        public bool? head { get; set; }

        [StringLength(500)]
        public string NavigateUrl { get; set; }

        public int? Position { get; set; }

        public DateTime? date { get; set; }

        public bool? AllowDelete { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public virtual ApplicationUser LastModifyUser { get; set; }
        public virtual ApplicationUser CreatorUser { get; set; }

    }
}
