using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FrotaMan.Models;

[Table("refuelling")]
public class RefuellingModel
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "Favor indicar o veículo")]
    public required int VehicleId { get; set; }

    [JsonIgnore]
    public VehicleModel? Fleet { get; set; }

    [Required]
    [Column(TypeName = "decimal(12,2)")]
    public decimal Cost { get; set; } = 0;

    [Required(ErrorMessage = "Favor indicar o momento em que o abastecimento ocorreu")]
    public DateTime FinishedAt { get; set; }

    [JsonPropertyName("odometer")]
    public int Odometer { get; set; } = 0;
}