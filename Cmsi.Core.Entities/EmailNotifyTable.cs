namespace AVA.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmailNotifyTable")]
    public partial class EmailNotifyTable
    {
        public int ID { get; set; }

        [StringLength(500)]
        public string Email { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
