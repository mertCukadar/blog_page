using System;
using System.Collections.Generic;

namespace blog_page.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public bool IsBanned { get; set; }

    public DateTime BannedDate { get; set; }

    public string? PasswordHash { get; set; }

    public string? PasswordSalt { get; set; }

    public virtual ICollection<BlogComment> BlogComments { get; set; } = new List<BlogComment>();

    public virtual ICollection<BlogLike> BlogLikes { get; set; } = new List<BlogLike>();

    public virtual ICollection<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();

    public virtual ICollection<FormPost> FormPosts { get; set; } = new List<FormPost>();

    public virtual ICollection<FormThread> FormThreads { get; set; } = new List<FormThread>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
