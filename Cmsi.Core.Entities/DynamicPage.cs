namespace AVA.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DynamicPage")]
    public partial class DynamicPage
    {
        public DynamicPage() {
            DynamicPageContents = new List<DynamicPageContent>();
            CreateDate = DateTime.Now;
        }
        public int ID { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        public bool? Enable { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreateDate { get; set; }

        public int? Priority { get; set; }

        public bool? AllowDelete { get; set; }
        public virtual ICollection<DynamicPageContent> DynamicPageContents { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public virtual ApplicationUser LastModifyUser { get; set; }
        public virtual ApplicationUser CreatorUser { get; set; }
    }
}
