namespace FrotaMan.Models;

public class TruckModel : VehicleModel
{
    public TruckModel() : base()
    {
        FuelType = FuelTypeEnum.diesel;
    }
}