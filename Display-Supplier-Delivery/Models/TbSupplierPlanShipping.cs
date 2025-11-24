using System;
using System.Collections.Generic;

namespace Display_Supplier_Delivery.Models;

public partial class TbSupplierPlanShipping
{
    public string SupId { get; set; }

    public string StrDay { get; set; }

    public string StartTime { get; set; }

    public string EndTime { get; set; }

    public int? TotalTime { get; set; }
}
