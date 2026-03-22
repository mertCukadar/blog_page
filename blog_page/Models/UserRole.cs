using System;
using System.Collections.Generic;

namespace blog_page.Models;

public partial class UserRole
{
    public int UserRolesId { get; set; }

    public int? RoleFkid { get; set; }

    public int? UserFkid { get; set; }

    public virtual Role? RoleFk { get; set; }

    public virtual User? UserFk { get; set; }
}
