using CloudTaskManager.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CloudTaskManager.Infrastructure;

public class CloudTaskDbContext : DbContext 
{
    public CloudTaskDbContext(DbContextOptions<CloudTaskDbContext> options) : base(options) { }

    public DbSet<TaskItem> Tasks { get; set; }
    public DbSet<Project> Projects { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>().HasKey(p => p.Id);
        modelBuilder.Entity<TaskItem>().HasKey(t => t.Id);

        modelBuilder.Entity<TaskItem>()
            .HasOne(t => t.Project)
            .WithMany(p => p.Tasks)
            .HasForeignKey(t => t.ProjectId);

        modelBuilder.Entity<Project>()
            .HasMany(p => p.Tasks)
            .WithOne(t => t.Project)
            .HasForeignKey(t => t.ProjectId);
    }
}