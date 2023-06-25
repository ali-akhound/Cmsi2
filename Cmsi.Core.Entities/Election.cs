namespace AVA.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Election")]
    public partial class Election
    {
        public int ID { get; set; }

        [StringLength(150)]
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Enable { get; set; } = true;
        [StringLength(350)]
        public string ElectionPosterUrl { get; set; }
        public string ElectionAttachUrl { get; set; }
        public virtual ICollection<Candidate> Candidates { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime? LastModifyDate { get; set; }
        public virtual ApplicationUser LastModifyUser { get; set; }
        public virtual ApplicationUser CreatorUser { get; set; }
    }
}
