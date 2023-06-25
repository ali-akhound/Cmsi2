namespace AVA.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RefereeAnswer")]
    public partial class RefereeAnswer
    {
        public int ID { get; set; }
        [StringLength(1000)]
        [Required]
        public string Answer { get; set; }
        [Required]
        public virtual RefereeQuestion RefereeQuestion { get; set; }
        [Required]
        public int Priority { get; set; }
        public bool Enable { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public virtual ApplicationUser LastModifyUser { get; set; }
        public virtual ApplicationUser CreatorUser { get; set; }
    }
}
