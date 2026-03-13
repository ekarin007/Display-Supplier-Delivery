using System;
using System.Collections.Generic;

namespace Display_Supplier_Delivery.Models;

public partial class TbRoleMenu
{
    public string RoleId { get; set; }

    public string MenuId { get; set; }

    public int IsActive { get; set; }

    public virtual TbMasterMainMenu Menu { get; set; }

    public virtual TbMasterRole Role { get; set; }
}
