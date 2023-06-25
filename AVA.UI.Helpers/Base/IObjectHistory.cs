using System;
using System.Collections.Generic;

namespace AVA.UI.Helpers.Base
{
    public interface IObjectHistory
    {
        string CreatedBy { get; set; }
        Nullable<DateTime> CreatedDate { get; set; }
        string ModifiedBy { get; set; }
        Nullable<DateTime> ModifiedDate { get; set; }
    }
}
