namespace FrotaMan.Data;

public interface IUnitOfWork
{
    public void Commit();
    public void Rollback();
}