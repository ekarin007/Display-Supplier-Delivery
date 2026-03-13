using System;
using System.Collections.Generic;

namespace Display_Supplier_Delivery.Models;

public partial class TbEmployee
{
    public string EmpId { get; set; }

    public string EmpNameEn { get; set; }

    public string EmpNameTh { get; set; }

    public string Department { get; set; }

    public string Department2 { get; set; }

    public string Scanner { get; set; }

    public string Report { get; set; }

    public string Active { get; set; }

    public byte[] Picture { get; set; }

    public byte[] Signature { get; set; }
}
