namespace AVA.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Article")]
    public partial class Article
    {
        public int ID { get; set; }
        [StringLength(500)]
        [Required]
        public string Title { get; set; }
        [StringLength(500)]
        [Required]
        public string EnglishTitle { get; set; }
        [Required]
        public string Summary { get; set; }
        [StringLength(500)]
        [Required]
        public string Keywords { get; set; }
        [StringLength(500)]
        public string ShortImageUrl { get; set; }
        [StringLength(500)]
        public string LongImageUrl { get; set; }
        [StringLength(500)]
        [Required]
        public string FileUrl { get; set; }
        [StringLength(500)]
        public string FileWordUrl { get; set; }
        [StringLength(500)]
        public string PosterUrl { get; set; }
        public DateTime? PresentTime { get; set; }
        public string PresentLocation { get; set; }
        public bool Enable { get; set; }
        public bool Published { get; set; }
        public bool? Visible { get; set; }
        public int? Visit { get; set; }
        public int? LikeCnt { get; set; }
        public virtual Language Language { get; set; }
        public virtual ArticleStatus ArticleStatus { get; set; }
        public virtual ArticlePresentType ArticlePresentType { get; set; }
        public string ArticlePresentTypeExplain { get; set; }
        public virtual ICollection<RefereeArticle> RefereeArticles { get; set; }
        public virtual ICollection<ArticleWriter> ArticleWriters { get; set; }
        public virtual ICollection<ArticleCategory> ArticleCategories { get; set; }
        public virtual ICollection<ConferenceArticle> ConferenceArticles { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public virtual ApplicationUser LastModifyUser { get; set; }
        public virtual ApplicationUser CreatorUser { get; set; }
    }
}
