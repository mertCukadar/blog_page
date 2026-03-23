using System;
using System.Collections.Generic;

namespace blog_page.Models;

public partial class BlogPost
{
    public int PostId { get; set; }

    public int AuthorFkuserId { get; set; }

    public int CategoryFkid { get; set; }

    public string Title { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public string BlogContent { get; set; } = null!;

    public bool Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime PublishedAt { get; set; }

    public int ViewCount { get; set; }

    public virtual User AuthorFkuser { get; set; } = null!;

    public virtual ICollection<BlogComment> BlogComments { get; set; } = new List<BlogComment>();

    public virtual ICollection<BlogLike> BlogLikes { get; set; } = new List<BlogLike>();

    public virtual BlogCategory CategoryFk { get; set; } = null!;
}
