namespace AVA.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ConferencePackage")]
    public partial class ConferencePackage
    {
        public int ID { get; set; }
        public virtual PackageName PackageName { get; set; }
        public virtual Conference Conference { get; set; }
        public decimal Price { get; set; } = 0;
        public DateTime CreateDate { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public virtual ApplicationUser LastModifyUser { get; set; }
        public virtual ApplicationUser CreatorUser { get; set; }
    }
}
