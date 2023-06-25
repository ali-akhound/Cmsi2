namespace AVA.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DynamicPageContent")]
    public partial class DynamicPageContent
    {
        public DynamicPageContent()
        {
            CreateDate = DateTime.Now;
        }
        public int ID { get; set; }


        [StringLength(100)]
        public string Title { get; set; }
        [StringLength(300)]
        public string Keyword { get; set; }


        [StringLength(300)]
        public string Description { get; set; }

        [Column(TypeName = "ntext")]
        public string Context { get; set; }

        public bool? Enable { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreateDate { get; set; }

        public int? Priority { get; set; }

        public bool? AllowDelete { get; set; }

        [Column(TypeName = "ntext")]
        public string DefaultContext { get; set; }
        public virtual DynamicPage DynamicPage { get; set; }
        public virtual Language Language { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public virtual ApplicationUser LastModifyUser { get; set; }
        public virtual ApplicationUser CreatorUser { get; set; }
    }
}
