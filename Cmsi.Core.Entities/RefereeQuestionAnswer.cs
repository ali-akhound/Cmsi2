namespace AVA.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RefereeQuestionAnswer")]
    public partial class RefereeQuestionAnswer
    {
        public long ID { get; set; }
        [Required]
        public virtual RefereeAnswer RefereeAnswer { get; set; }
        [Required]
        public virtual RefereeArticle RefereeArticle { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public virtual ApplicationUser LastModifyUser { get; set; }
        public virtual ApplicationUser CreatorUser { get; set; }
    }
}
