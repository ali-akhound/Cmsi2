namespace AVA.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProductTable")]
    public partial class ProductTable
    {
        public int ID { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        [Column(TypeName = "ntext")]
        public string Explain { get; set; }

        public int? CatID { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(300)]
        public string ShortImageUrl { get; set; }

        [StringLength(300)]
        public string LongImageUrl { get; set; }

        [StringLength(4000)]
        public string ExplainSummery { get; set; }

        public int? Position { get; set; }

        [StringLength(500)]
        public string PageKeyword { get; set; }

        [StringLength(500)]
        public string PageDesc { get; set; }

        public bool? Visible { get; set; }

        public decimal? Price { get; set; }

        public decimal? OldPrice { get; set; }

        public byte? StockState { get; set; }

        [StringLength(500)]
        public string Publisher { get; set; }

        public int? FK_SendTypeID { get; set; }

        [StringLength(500)]
        public string DownloadUrl { get; set; }

        public int? Capacity { get; set; }

        public bool? Enable { get; set; }

        public int? Visit { get; set; }
    }
}
