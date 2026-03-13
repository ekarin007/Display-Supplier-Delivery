using System;
using System.Collections.Generic;

namespace Display_Supplier_Delivery.Models;

public partial class TbMasterRole
{
    public string RoleId { get; set; }

    public string RoleName { get; set; }

    public string Description { get; set; }

    public int? IsActive { get; set; }

    public virtual ICollection<TbRoleMenu> TbRoleMenus { get; set; } = new List<TbRoleMenu>();
}
