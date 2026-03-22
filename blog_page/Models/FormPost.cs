using System;
using System.Collections.Generic;

namespace blog_page.Models;

public partial class FormPost
{
    public int FormPostId { get; set; }

    public int? ThreadsFkid { get; set; }

    public int? AuthorFkid { get; set; }

    public string? ThreadContent { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual User? AuthorFk { get; set; }

    public virtual FormThread? ThreadsFk { get; set; }
}
