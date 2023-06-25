namespace AVA.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PollQuestion")]
    public partial class PollQuestion
    {
        public int ID { get; set; }
        [StringLength(1000)]
        [Required]
        public string Question { get; set; }
        [Required]
        public int Priority { get; set; }
        public bool Enable { get; set; }
        public virtual Poll Poll { get; set; }
        public virtual Language Language { get; set; }
        public virtual ICollection<PollAnswer> PollAnswers { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public virtual ApplicationUser LastModifyUser { get; set; }
        public virtual ApplicationUser CreatorUser { get; set; }
    }
}
