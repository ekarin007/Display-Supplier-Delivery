using System;
using System.Collections.Generic;

namespace Display_Supplier_Delivery.Models;

public partial class TbMstPartNo
{
    public string PartNo { get; set; }

    public string Supplier { get; set; }

    public string Units { get; set; }

    public int? LotSize { get; set; }

    public string Location { get; set; }

    public string Status { get; set; }
}
