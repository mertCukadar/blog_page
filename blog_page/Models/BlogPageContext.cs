using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace blog_page.Models;

public partial class BlogPageContext : DbContext
{
    public BlogPageContext()
    {
    }

    public BlogPageContext(DbContextOptions<BlogPageContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BlogCategory> BlogCategories { get; set; }

    public virtual DbSet<BlogComment> BlogComments { get; set; }

    public virtual DbSet<BlogLike> BlogLikes { get; set; }

    public virtual DbSet<BlogPost> BlogPosts { get; set; }

    public virtual DbSet<FormCategory> FormCategories { get; set; }

    public virtual DbSet<FormPost> FormPosts { get; set; }

    public virtual DbSet<FormThread> FormThreads { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-H5JE9MN;Database=blog_page;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BlogCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId);

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CreatedAt).HasColumnName("Created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Slug)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<BlogComment>(entity =>
        {
            entity.HasKey(e => e.CommentId);

            entity.Property(e => e.CommentId).HasColumnName("CommentID");
            entity.Property(e => e.AuthorsFkuserId).HasColumnName("AuthorsFKUserID");
            entity.Property(e => e.BlogPostFkid).HasColumnName("BlogPostFKID");
            entity.Property(e => e.Comment)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsDeleted).HasColumnName("Is_deleted");

            entity.HasOne(d => d.AuthorsFkuser).WithMany(p => p.BlogComments)
                .HasForeignKey(d => d.AuthorsFkuserId)
                .HasConstraintName("FK_BlogComments_Users");

            entity.HasOne(d => d.BlogPostFk).WithMany(p => p.BlogComments)
                .HasForeignKey(d => d.BlogPostFkid)
                .HasConstraintName("FK_BlogComments_BlogPosts");
        });

        modelBuilder.Entity<BlogLike>(entity =>
        {
            entity.HasKey(e => e.LikeId);

            entity.Property(e => e.LikeId).HasColumnName("LikeID");
            entity.Property(e => e.PostFkid).HasColumnName("PostFKID");
            entity.Property(e => e.UserFkid).HasColumnName("UserFKID");

            entity.HasOne(d => d.PostFk).WithMany(p => p.BlogLikes)
                .HasForeignKey(d => d.PostFkid)
                .HasConstraintName("FK_BlogLikes_BlogPosts");

            entity.HasOne(d => d.UserFk).WithMany(p => p.BlogLikes)
                .HasForeignKey(d => d.UserFkid)
                .HasConstraintName("FK_BlogLikes_Users");
        });

        modelBuilder.Entity<BlogPost>(entity =>
        {
            entity.HasKey(e => e.PostId);

            entity.Property(e => e.PostId).HasColumnName("PostID");
            entity.Property(e => e.AuthorFkuserId).HasColumnName("AuthorFKUserID");
            entity.Property(e => e.BlogContent).HasColumnType("text");
            entity.Property(e => e.CategoryFkid).HasColumnName("CategoryFKID");
            entity.Property(e => e.Slug)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("slug");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.AuthorFkuser).WithMany(p => p.BlogPosts)
                .HasForeignKey(d => d.AuthorFkuserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BlogPosts_Users");

            entity.HasOne(d => d.CategoryFk).WithMany(p => p.BlogPosts)
                .HasForeignKey(d => d.CategoryFkid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BlogPosts_BlogCategories");
        });

        modelBuilder.Entity<FormCategory>(entity =>
        {
            entity.HasKey(e => e.FormCategorieId);

            entity.Property(e => e.FormCategorieId).HasColumnName("FormCategorieID");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Slug)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<FormPost>(entity =>
        {
            entity.Property(e => e.FormPostId).HasColumnName("FormPostID");
            entity.Property(e => e.AuthorFkid).HasColumnName("AuthorFKID");
            entity.Property(e => e.ThreadContent).IsUnicode(false);
            entity.Property(e => e.ThreadsFkid).HasColumnName("ThreadsFKID");

            entity.HasOne(d => d.AuthorFk).WithMany(p => p.FormPosts)
                .HasForeignKey(d => d.AuthorFkid)
                .HasConstraintName("FK_FormPosts_Users");

            entity.HasOne(d => d.ThreadsFk).WithMany(p => p.FormPosts)
                .HasForeignKey(d => d.ThreadsFkid)
                .HasConstraintName("FK_FormPosts_FormThreads");
        });

        modelBuilder.Entity<FormThread>(entity =>
        {
            entity.HasKey(e => e.ThreadId);

            entity.Property(e => e.ThreadId).HasColumnName("ThreadID");
            entity.Property(e => e.AuthhorFkid).HasColumnName("AuthhorFKID");
            entity.Property(e => e.FormCategoryFkid).HasColumnName("FormCategoryFKID");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.AuthhorFk).WithMany(p => p.FormThreads)
                .HasForeignKey(d => d.AuthhorFkid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FormThreads_Users");

            entity.HasOne(d => d.FormCategoryFk).WithMany(p => p.FormThreads)
                .HasForeignKey(d => d.FormCategoryFkid)
                .HasConstraintName("FK_FormThreads_FormCategories");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.ToTable("Menu");

            entity.Property(e => e.MenuId).HasColumnName("MenuID");
            entity.Property(e => e.MenuAdi)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MenuAdresi).IsUnicode(false);
            entity.Property(e => e.MenuFkid).HasColumnName("MenuFKID");
            entity.Property(e => e.MenuRolFkid).HasColumnName("MenuRolFKID");

            entity.HasOne(d => d.MenuFk).WithMany(p => p.InverseMenuFk)
                .HasForeignKey(d => d.MenuFkid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Menu_Menu");

            entity.HasOne(d => d.MenuRolFk).WithMany(p => p.Menus)
                .HasForeignKey(d => d.MenuRolFkid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Menu_Roles");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BannedDate).HasColumnName("Banned_date");
            entity.Property(e => e.CreatedAt).HasColumnName("Created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsBanned).HasColumnName("Is_banned");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.UserRolesId);

            entity.Property(e => e.UserRolesId).HasColumnName("UserRolesID");
            entity.Property(e => e.RoleFkid).HasColumnName("RoleFKID");
            entity.Property(e => e.UserFkid).HasColumnName("UserFKID");

            entity.HasOne(d => d.RoleFk).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleFkid)
                .HasConstraintName("FK_UserRoles_Roles");

            entity.HasOne(d => d.UserFk).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserFkid)
                .HasConstraintName("FK_UserRoles_Users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
