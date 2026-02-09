using FrotaMan.Models;

namespace FrotaMan.Repositories;

public interface IMaintenanceRepository
{
    public void Save(MaintenanceModel maintenanceData);
}