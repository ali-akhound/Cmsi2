namespace AVA.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SocietyVipOrder")]
    public partial class SocietyVipOrder
    {
        public int ID { get; set; }
        [Required]
        public virtual PackageName PackageName { get; set; }
        [Required]
        public virtual OrderStatus OrderStatus { get; set; }
        [Required]
        public int Count { get; set; }       
        public string UniversityCardUrl { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public virtual ApplicationUser LastModifyUser { get; set; }
        public virtual ApplicationUser CreatorUser { get; set; }
    }
}
