using System;
using System.Collections.Generic;

namespace Display_Supplier_Delivery.Models;

public partial class TbKanban
{
    public string Kanban { get; set; }

    public string PartNo { get; set; }

    public int QtyPlanReceive { get; set; }

    public int QtyReceive { get; set; }

    public int QtyOutLocation { get; set; }

    public int QtyBalance { get; set; }

    public string Location { get; set; }

    public string CurrentStatus { get; set; }

    public string DeliveryNo { get; set; }

    public DateTime? DatePlan { get; set; }

    public DateTime? DateReceive { get; set; }
}
