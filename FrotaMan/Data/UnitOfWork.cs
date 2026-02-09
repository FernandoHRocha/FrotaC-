namespace FrotaMan.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly FleetContext _fleetContext;

    public UnitOfWork(FleetContext dbContext)
    {
        _fleetContext = dbContext;
    }

    public void Commit()
    {
        _fleetContext.SaveChanges();
    }

    public void Rollback()
    {
        Console.WriteLine("Algumas modificações não foram efetivadas no banco de dados");
    }
}