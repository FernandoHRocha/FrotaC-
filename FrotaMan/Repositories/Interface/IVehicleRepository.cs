using FrotaMan.Models;

namespace FrotaMan.Repositories;

public interface IVehicleRepository
{
    public Task<VehicleModel?> Get(int vehicleId);
    public List<VehicleModel> List();
    public void Save(VehicleModel vehicleData);
}