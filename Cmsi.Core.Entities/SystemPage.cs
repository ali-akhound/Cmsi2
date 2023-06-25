namespace AVA.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SystemPage")]
    public partial class SystemPage
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(150)]
        public string Address { get; set; }

        [StringLength(800)]
        public string CreateDate { get; set; }

        [StringLength(800)]
        public string Explain { get; set; }

        public int? SystemType { get; set; }

        public int? GroupID { get; set; }
    }
}
