using System;
using System.Collections.Generic;

namespace blog_page.Models;

public partial class BlogComment
{
    public int CommentId { get; set; }

    public int? BlogPostFkid { get; set; }

    public int? AuthorsFkuserId { get; set; }

    public string? Comment { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual User? AuthorsFkuser { get; set; }

    public virtual BlogPost? BlogPostFk { get; set; }
}
