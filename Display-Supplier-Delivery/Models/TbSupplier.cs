using System;
using System.Collections.Generic;

namespace Display_Supplier_Delivery.Models;

public partial class TbSupplier
{
    public string SupId { get; set; }

    public string SupNameTh { get; set; }

    public string SupNameEn { get; set; }

    public string Factory { get; set; }

    public string LoadingArea { get; set; }

    public string Cycle { get; set; }
}
