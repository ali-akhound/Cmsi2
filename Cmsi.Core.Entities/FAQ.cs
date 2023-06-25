namespace AVA.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FAQ")]
    public partial class FAQ
    {
        public int ID { get; set; }
        [StringLength(500)]
        public string Question { get; set; }
        public string Answer { get; set; }
        public bool? Enable { get; set; }
        public int Priority { get; set; }
        public virtual Language Language { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public virtual ApplicationUser LastModifyUser { get; set; }
        public virtual ApplicationUser CreatorUser { get; set; }
    }
}
