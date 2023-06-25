namespace AVA.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ContactUs
    {
        public long ID { get; set; }

        [StringLength(500)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Family { get; set; }

        [StringLength(50)]
        public string TelNumber { get; set; }

        [StringLength(50)]
        public string Email { get; set; }
        [StringLength(200)]
        public string Subject { get; set; }

        public string Text { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
