using System;
using System.Collections.Generic;

namespace blog_page.Models;

public partial class FormThread
{
    public int ThreadId { get; set; }

    public int AuthhorFkid { get; set; }

    public int? FormCategoryFkid { get; set; }

    public string? Title { get; set; }

    public bool? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User AuthhorFk { get; set; } = null!;

    public virtual FormCategory? FormCategoryFk { get; set; }

    public virtual ICollection<FormPost> FormPosts { get; set; } = new List<FormPost>();
}
