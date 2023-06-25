namespace AVA.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PollUserAnswer")]
    public partial class PollUserAnswer
    {
        public int ID { get; set; }

        public long? FK_UserID { get; set; }

        public long? FK_AnswerID { get; set; }

        public DateTime? Date { get; set; }

        public virtual PollAnswer PollAnswer { get; set; }
    }
}
