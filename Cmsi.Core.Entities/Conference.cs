namespace AVA.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Conference")]
    public partial class Conference
    {
        public int ID { get; set; }
        [StringLength(500)]
        [Required]
        public string Title { get; set; }
        [StringLength(500)]
        public string EnglishTitle { get; set; }
        [Required]
        public DateTime SendStartDate { get; set; }
        [Required]
        public DateTime SendEndDate { get; set; }
        [Required]
        public DateTime EventDate { get; set; }
        public string PosterImageUrl { get; set; }
        public string AttachFileUrl { get; set; }
        public string AttachAmayeshFileUrl { get; set; }
        public string AttachExecutiveCommitteeFileUrl { get; set; }
        public string AttachScientificCommitteeFileUrl { get; set; }
        public string AttachPresentationHelpFileUrl { get; set; }
        public string AttachPresentationPowerpointFileUrl { get; set; }
        public string AttachPosterTemplateFileUrl { get; set; }
        public string AttachPhysicsPresentationProgramFileUrl { get; set; }
        public string AttachChemistryPresentationProgramFileUrl { get; set; }
        public string AttachGeologyPresentationProgramFileUrl { get; set; }
        public string AttachOpeningPlanUrl { get; set; }
        public string AttachAttendingHelpUrl { get; set; }
        public string AttachTotalArticlesFileUrl { get; set; }
        
        public string MobileTel { get; set; }
        public string Explain { get; set; }
        public string Place { get; set; }
        public string TelegramUrl { get; set; }        
        public bool Enable { get; set; }
        public bool Visible { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public virtual ApplicationUser LastModifyUser { get; set; }
        public virtual ApplicationUser CreatorUser { get; set; }
    }
}
