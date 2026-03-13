using System;
using System.Collections.Generic;

namespace Display_Supplier_Delivery.Models;

public partial class ControlChartDetail
{
    public int Id { get; set; }

    public int HeaderId { get; set; }

    public string SpecName { get; set; }

    public string Value1 { get; set; }

    public string Value2 { get; set; }

    public string Value3 { get; set; }

    public string Value4 { get; set; }

    public string Value5 { get; set; }

    public string Status { get; set; }

    public virtual ControlChartHeader Header { get; set; }
}
