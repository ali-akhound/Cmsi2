namespace AVA.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PollAnswer")]
    public partial class PollAnswer
    {
        public int ID { get; set; }
        [StringLength(1000)]
        [Required]
        public string Answer { get; set; }
        public virtual Language Language { get; set; }
        [Required]
        public virtual PollQuestion PollQuestion { get; set; }
        [Required]
        public int Priority { get; set; }
        public bool Enable { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public virtual ApplicationUser LastModifyUser { get; set; }
        public virtual ApplicationUser CreatorUser { get; set; }
    }
}
