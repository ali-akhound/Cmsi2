namespace AVA.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ArticleCategory")]
    public partial class ArticleCategory
    {
        public int ID { get; set; }
        [Required]
        public virtual Article Article { get; set; }
        [Required]
        public virtual Category Category { get; set; }
        public bool Enable { get; set; }
        public DateTime CreateDate { get; set; }        
        public DateTime? LastModifyDate { get; set; }
        public virtual ApplicationUser LastModifyUser { get; set; }
        public virtual ApplicationUser CreatorUser { get; set; }
    }
}
