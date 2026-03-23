using System;
using System.Collections.Generic;

namespace blog_page.Models;

public partial class FormCategory
{
    public int FormCategorieId { get; set; }

    public string? Name { get; set; }

    public string? Slug { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<FormThread> FormThreads { get; set; } = new List<FormThread>();
}
