namespace AVA.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MailTemplate
    {
        public int ID { get; set; }

        public string Params { get; set; }

        public string Template { get; set; }

        [StringLength(500)]
        public string Subject { get; set; }

        public string SMS { get; set; }

        public DateTime CreateDate { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public virtual ApplicationUser LastModifyUser { get; set; }
        public virtual ApplicationUser CreatorUser { get; set; }
    }
}
