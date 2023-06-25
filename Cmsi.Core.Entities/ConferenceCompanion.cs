namespace AVA.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ConferenceCompanion")]
    public partial class ConferenceCompanion
    {
        public int ID { get; set; }
        [StringLength(500)]
        [Required]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(100)]
        public string University { get; set; }

        [StringLength(100)]
        public string FieldOfStudy { get; set; }

        [StringLength(100)]
        public string Degree { get; set; }

        public DateTime? BornDate { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(150)]
        public string PersonalImageUrl { get; set; }

        [StringLength(300)]
        public string PayReceiptUrl { get; set; }

        [StringLength(300)]
        public string UniversityCardUrl { get; set; }

        [StringLength(10)]
        public string Melicode { get; set; }
        [StringLength(11)]
        public string Cellphone { get; set; }

        public bool Enable { get; set; }
        public virtual Conference Conference { get; set; }
        [Required]
        public virtual ConferencePackage ConferencePackage { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public virtual ApplicationUser LastModifyUser { get; set; }
        public virtual ApplicationUser CreatorUser { get; set; }
    }
}
