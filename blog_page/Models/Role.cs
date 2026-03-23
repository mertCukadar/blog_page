using System;
using System.Collections.Generic;

namespace blog_page.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Menu> Menus { get; set; } = new List<Menu>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
