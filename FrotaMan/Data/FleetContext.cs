using Microsoft.EntityFrameworkCore;
using FrotaMan.Models;

namespace FrotaMan.Data;

public class FleetContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VehicleModel>()
            .HasDiscriminator<string>("type")
            .HasValue<CarModel>(nameof(VehicleTypeEnum.car))
            .HasValue<MotorcycleModel>(nameof(VehicleTypeEnum.motorcycle))
            .HasValue<TruckModel>(nameof(VehicleTypeEnum.truck));

        modelBuilder.Entity<VehicleModel>().UseTphMappingStrategy();

        modelBuilder.Entity<VehicleModel>()
            .HasMany(v => v.Maintenances)
            .WithOne(m => m.Fleet)
            .HasForeignKey(m => m.VehicleId);
        
        modelBuilder.Entity<VehicleModel>()
            .HasMany(v => v.Refuellings)
            .WithOne(r => r.Fleet)
            .HasForeignKey(r => r.VehicleId);
    }
    public FleetContext(DbContextOptions<FleetContext> options) : base(options)
    {
        
    }
    public DbSet<VehicleModel> Vehicles { get; set; }
    public DbSet<MaintenanceModel> Maintenances { get; set; }
    public DbSet<RefuellingModel> Refuellings { get; set; }
}