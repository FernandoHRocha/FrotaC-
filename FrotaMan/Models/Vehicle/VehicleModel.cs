using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FrotaMan.Models;



[JsonPolymorphic(TypeDiscriminatorPropertyName = "type", UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization)]
[JsonDerivedType(typeof(CarModel), typeDiscriminator: nameof(VehicleTypeEnum.car))]
[JsonDerivedType(typeof(MotorcycleModel), typeDiscriminator: nameof(VehicleTypeEnum.motorcycle))]
[JsonDerivedType(typeof(TruckModel), typeDiscriminator: nameof(VehicleTypeEnum.truck))]
[Table("vehicle")]
abstract public class VehicleModel
{
    [Key]
    [Required]
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Favor indicar o fabricante do veículo")]
    [MaxLength(255, ErrorMessage = "O nome do fabricante do veículo deve conter entre 3 e 255 caracteres")]
    [MinLength(3, ErrorMessage = "O nome do fabricante do veículo deve conter entre 3 e 255 caracteres")]
    [JsonPropertyName("manufacturer")]
    public required string Manufacturer { get; set; }

    [Required(ErrorMessage = "Favor indicar o modelo do veículo")]
    [JsonPropertyName("model")]
    public required string Model { get; set; }

    [Required(ErrorMessage = "Favor indicar o ano de fabricação")]
    [Range(1800, 2100, ErrorMessage = "O ano deve estar entre 1800 e 2100")]
    [JsonPropertyName("year")]
    public required int Year { get; set; }

    [Required(ErrorMessage = "Favor indicar o código que identifica o veículo")]
    [StringLength(7, MinimumLength = 7, ErrorMessage = "A placa deve ter exatamente 7 caracteres")]
    [RegularExpression(@"^[a-zA-Z0-9]{7}$", ErrorMessage = "A placa deve conter apenas 7 caracteres alfanuméricos")]
    [JsonPropertyName("licensePlate")]
    public required string LicensePlate { get; set
        {
            if (value.Count() != 7)
            {
                throw new ArgumentException("A placa deve ter exatamente 7 caracteres");
            }
            field = value;
        }
    }

    [JsonPropertyName("odometer")]
    public int Odometer { get; set; } = 0;

    [Required(ErrorMessage = "Favor indicar qual o tipo de combustível")]
    [JsonPropertyName("fuelType")]
    public required FuelTypeEnum FuelType { get; set; }

    [Required(ErrorMessage = "Favor indicar o volume do tanque de combustível")]
    [JsonPropertyName("tankVolume")]
    [Range(2, 1000, ErrorMessage = "O volume do tanque deve estar entre 2 e 1000")]
    public required int TankVolume { get; set; } = 0;

    public List<MaintenanceModel> Maintenances { get; set; } = new List<MaintenanceModel>();
    public List<RefuellingModel> Refuellings { get; set; } = new List<RefuellingModel>();

    public override string ToString()
    {
        return $"Veículo tipo {this.GetType()}, {Manufacturer} modelo {Model} {Year} placa {LicensePlate}";
    }

    public MaintenanceModel CreateMaintenance(decimal cost, string serviceDescription = "Manutenção preventiva")
    {
        var maintenance = new MaintenanceModel
        {
            VehicleId = this.Id,
            Cost = cost,
            FinishedAt = DateTime.Now,
            Odometer = this.Odometer,
            Fleet = this,
            ServiceDescription = serviceDescription
        };
        this.Maintenances.Add(maintenance);
        return maintenance;
    }
}