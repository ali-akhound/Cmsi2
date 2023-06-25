namespace AVA.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SysModule")]
    public partial class SysModule
    {
        public int ID { get; set; }

        [Column("Name")]
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(50)]
        public string PersianName { get; set; }
    }
}
