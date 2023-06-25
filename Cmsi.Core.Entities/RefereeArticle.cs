namespace AVA.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RefereeArticle")]
    public partial class RefereeArticle
    {
        public int ID { get; set; }
        [Required]
        public virtual RefereeStatus RefereeStatus { get; set; }
        [Required]
        public virtual Article Article { get; set; }
        [Required]
        public virtual Referee Referee { get; set; }
        [Required]
        public virtual ArticlePresentType ArticlePresentType { get; set; }
        public string Explain { get; set; }
        [StringLength(250)]
        public string AttachUrl { get; set; }
        public bool IsRead { get; set; }
        public bool IsArchived { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public virtual ApplicationUser LastModifyUser { get; set; }
        public virtual ApplicationUser CreatorUser { get; set; }
    }
}
