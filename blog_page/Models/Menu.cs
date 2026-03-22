using System;
using System.Collections.Generic;

namespace blog_page.Models;

public partial class Menu
{
    public int MenuId { get; set; }

    public int MenuFkid { get; set; }

    public string MenuAdi { get; set; } = null!;

    public string MenuAdresi { get; set; } = null!;

    public int? MenuSirasi { get; set; }

    public int MenuRolFkid { get; set; }

    public virtual ICollection<Menu> InverseMenuFk { get; set; } = new List<Menu>();

    public virtual Menu MenuFk { get; set; } = null!;

    public virtual Role MenuRolFk { get; set; } = null!;
}
