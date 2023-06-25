namespace AVA.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SocietyMember")]
    public partial class SocietyMember
    {
        public int ID { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(100)]
        public string Family { get; set; }
        [StringLength(50)]
        public string Email { get; set; }
        [StringLength(50)]
        public string Position { get; set; }
        [StringLength(100)]
        public string ResumeUrl { get; set; }
        [StringLength(100)]
        public string PersonalImageUrl { get; set; }
        [StringLength(50)]
        public string DegreeLevel { get; set; }
        public bool Enable { get; set; }
        public virtual SocietyMemberPeriod SocietyMemberPeriod { get; set; }
        public virtual Language Language { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public virtual ApplicationUser LastModifyUser { get; set; }
        public virtual ApplicationUser CreatorUser { get; set; }
    }
}
