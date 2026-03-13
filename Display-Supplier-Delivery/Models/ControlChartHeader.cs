using System;
using System.Collections.Generic;

namespace Display_Supplier_Delivery.Models;

public partial class ControlChartHeader
{
    public int Id { get; set; }

    public DateOnly InspectionDate { get; set; }

    public string RoundTime { get; set; }

    public string ProductionLine { get; set; }

    public string ProductionProcess { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? Status { get; set; }

    public string CreatedBy { get; set; }

    public string Leader { get; set; }

    public string Inspector { get; set; }

    public virtual ICollection<ControlChartDetail> ControlChartDetails { get; set; } = new List<ControlChartDetail>();
}
