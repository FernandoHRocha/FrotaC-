using FrotaMan.Models;

namespace FrotaMan.Repositories;

public interface IRefuellingRepository
{
    public void Save(RefuellingModel refuellingData);
}