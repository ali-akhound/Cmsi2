namespace AVA.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Invoice")]
    public partial class Invoice
    {
        public int ID { get; set; }
        public int DocumentID { get; set; }
        [StringLength(100)]
        public string TableName { get; set; }
        [StringLength(200)]
        public string DigitalCode { get; set; }
        bool Confirm { get; set; }
        [StringLength(200)]
        public string BankName { get; set; }
        [StringLength(200)]
        public string ReservationCode { get; set; }
        [StringLength(200)]
        public string Last4digitCardNumber { get; set; }
        [StringLength(200)]
        public string Card2CardPeygiri { get; set; }
        public DateTime? Card2CardPayDate { get; set; }
        [StringLength(200)]
        public string Cash2BankPayReceipt { get; set; }
        public DateTime? Cash2BankPayDate { get; set; }
        [StringLength(300)]
        public string Cash2BankPayFileUrl { get; set; }
        public virtual InvoiceType InvoiceType { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public virtual ApplicationUser LastModifyUser { get; set; }
        public virtual ApplicationUser CreatorUser { get; set; }
    }
}
