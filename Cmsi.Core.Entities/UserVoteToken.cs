namespace AVA.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserVoteToken")]
    public partial class UserVoteToken
    {
        public int ID { get; set; }
        public string Token { get; set; }
        public virtual Election Election { get; set; }
        public virtual ApplicationUser CreatorUser { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime? LastModifyDate { get; set; }
        public virtual ApplicationUser LastModifyUser { get; set; }
    }
}
