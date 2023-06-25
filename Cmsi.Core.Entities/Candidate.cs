namespace AVA.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Candidate")]
    public partial class Candidate
    {
        public int ID { get; set; }

        [StringLength(150)]
        public string FirstName { get; set; }

        [StringLength(150)]
        public string LastName { get; set; }
        [StringLength(250)]
        public string Degree { get; set; }
        [StringLength(250)]
        public string University { get; set; }
        [StringLength(250)]
        public string FieldOfStudy { get; set; }
        [StringLength(500)]
        public string PersonPic { get; set; }
        [StringLength(500)]
        public string ResumeUrl { get; set; }
        [StringLength(250)]
        public string Explain { get; set; }
        public bool Enable { get; set; } = true;
        public virtual Election Election { get; set; }
        public virtual CandidateType CandidateType { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime? LastModifyDate { get; set; }
        public virtual ApplicationUser LastModifyUser { get; set; }
        public virtual ApplicationUser CreatorUser { get; set; }
    }
}
