namespace AVA.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ScientificSecretary")]
    public partial class ScientificSecretary
    {
        public int ID { get; set; }
        public bool Enable { get; set; }
        public ApplicationUser RefrenceUser { get; set; }
        public DateTime CreateDate { get; set; }        
        public DateTime? LastModifyDate { get; set; }
        public virtual ApplicationUser LastModifyUser { get; set; }
        public virtual ApplicationUser CreatorUser { get; set; }
    }
}
