using App.Domain;
using App.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public DbSet<AppUser> AppUsers { get; set; } = default!;
    public DbSet<AppRole> AppRoles { get; set; } = default!;
    public DbSet<Comment> Comments { get; set; } = default!;
    public DbSet<CommentReaction> CommentReactions { get; set; } = default!;
    public DbSet<DomainReport> DomainReports { get; set; } = default!;
    public DbSet<Url> Urls { get; set; } = default!;
    public DbSet<WebDomain> WebDomains { get; set; } = default!;
    public DbSet<Message> Messages { get; set; } = default!;
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
