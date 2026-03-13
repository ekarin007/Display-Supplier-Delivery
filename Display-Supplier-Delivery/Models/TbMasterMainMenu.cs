using System;
using System.Collections.Generic;

namespace Display_Supplier_Delivery.Models;

public partial class TbMasterMainMenu
{
    public string MenuId { get; set; }

    public string MenuName { get; set; }

    public int IsActive { get; set; }

    public virtual ICollection<TbRoleMenu> TbRoleMenus { get; set; } = new List<TbRoleMenu>();
}
