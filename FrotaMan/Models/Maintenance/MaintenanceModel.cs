using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FrotaMan.Models;

[Table("maintenance")]
public class MaintenanceModel
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
    
    [Required]
    [StringLength(255, MinimumLength = 8, ErrorMessage = "A descrição precisa ter entre 8 e 255 letras.")]
    public string ServiceDescription { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Favor indicar a data em que o serviço foi iniciado")]
    public DateTime StartedAt { get; set; }
    
    [Required(ErrorMessage = "Favor indicar a data em que o serviço foi finalizado")]
    public DateTime FinishedAt { get; set; }

    [JsonPropertyName("odometer")]
    public int Odometer { get; set; } = 0;

    public override string ToString()
    {
        return $"Veículo: {Fleet?.Model}, Custo: {Cost}, ServiceDescription: {ServiceDescription}, Finalizada em: {FinishedAt}";
    }
}