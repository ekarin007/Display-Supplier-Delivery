using System;
using System.Collections.Generic;

namespace Display_Supplier_Delivery.Models;

public partial class TbHistory
{
    public string Kanban { get; set; }

    public string PartNo { get; set; }

    public string Status { get; set; }

    public string Location { get; set; }

    public DateTime? EventDate { get; set; }

    public string UserId { get; set; }
}
