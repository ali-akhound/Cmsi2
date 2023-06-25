namespace AVA.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserAnswer")]
    public partial class UserAnswer
    {
        public int ID { get; set; }

        public long? FK_UserID { get; set; }

        public long? FK_AnswerID { get; set; }

        public DateTime? Date { get; set; }

        public bool? Winner { get; set; }

        [StringLength(10)]
        public string RahgiriCode { get; set; }

        public virtual ApplicationUser Users { get; set; }
    }
}
