using System;
using System.Collections.Generic;

namespace blog_page.Models;

public partial class BlogLike
{
    public int LikeId { get; set; }

    public int? PostFkid { get; set; }

    public int? UserFkid { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual BlogPost? PostFk { get; set; }

    public virtual User? UserFk { get; set; }
}
