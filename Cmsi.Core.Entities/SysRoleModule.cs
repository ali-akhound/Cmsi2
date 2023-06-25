namespace AVA.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SysRoleModule")]
    public partial class SysRoleModule
    {
        public int ID { get; set; }
        public virtual SysModule Module { get; set; }
        public virtual AspNetRole Role { get; set; }
    }
}
