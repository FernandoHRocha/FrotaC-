using FrotaMan.Data;
using FrotaMan.Models;

namespace FrotaMan.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly FleetContext _fleetContext;
    public VehicleRepository(FleetContext dbContext)
    {
        _fleetContext = dbContext;
    }

    public async Task<VehicleModel?> Get(int vehicleId)
    {
        return await _fleetContext.Vehicles.FindAsync(vehicleId);
    }

    public List<VehicleModel> List()
    {
        return _fleetContext.Vehicles.ToList();
    }

    public void Save(VehicleModel vehicleData)
    {
        _fleetContext.Vehicles.Add(vehicleData);
    }
}