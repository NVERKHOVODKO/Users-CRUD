using Microsoft.EntityFrameworkCore;
using TestApplication.Models;

namespace TestApplication.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        /*Database.EnsureDeleted();
        Database.EnsureCreated();*/
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserRoleModel>()
            .HasOne(u => u.UserModel)
            .WithMany(ur => ur.UserRoleModels)
            .HasForeignKey(ui => ui.UserId);
        
        modelBuilder.Entity<UserRoleModel>()
            .HasOne(r => r.RoleModel)
            .WithMany(ur => ur.UserRoleModels)
            .HasForeignKey(ri => ri.RoleId);
        
        modelBuilder.Entity<UserModel>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
    
    public DbSet<UserModel> Users { get; set; }
    public DbSet<RoleModel> Roles { get; set; }
    public DbSet<UserRoleModel> UserRoles { get; set; }
}