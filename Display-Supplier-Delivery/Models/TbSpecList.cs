using System;
using System.Collections.Generic;

namespace Display_Supplier_Delivery.Models;

public partial class TbSpecList
{
    public string LocationInspect { get; set; }

    public string ItemList { get; set; }

    public string SpecValue { get; set; }

    public double? Spec { get; set; }

    public double? MinSpec { get; set; }

    public double? MaxSpec { get; set; }
}
