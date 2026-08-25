using FastScan.Models;
using Microsoft.EntityFrameworkCore;

namespace FastScan.Services;

public class FastScanDbContext(DbContextOptions<FastScanDbContext> options) : DbContext(options)
{
    public DbSet<Branch> Branches => Set<Branch>(); public DbSet<Product> Products => Set<Product>(); public DbSet<SerializedUnit> SerializedUnits => Set<SerializedUnit>(); public DbSet<InventoryMovement> InventoryMovements => Set<InventoryMovement>(); public DbSet<InventoryMovementItem> InventoryMovementItems => Set<InventoryMovementItem>(); public DbSet<User> Users => Set<User>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    { base.OnModelCreating(modelBuilder); modelBuilder.Entity<Branch>().HasIndex(x => x.Code).IsUnique(); modelBuilder.Entity<Product>().HasIndex(x => x.Sku).IsUnique(); modelBuilder.Entity<Product>().HasIndex(x => x.Ean).IsUnique(); modelBuilder.Entity<SerializedUnit>().HasIndex(x => x.SerialNumber).IsUnique(); modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique(); modelBuilder.Entity<InventoryMovement>().HasMany(x => x.Items).WithOne().HasForeignKey(x => x.InventoryMovementId).OnDelete(DeleteBehavior.Cascade); }
}
