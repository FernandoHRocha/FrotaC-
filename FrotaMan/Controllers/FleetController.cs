using System.Text.Json;
using FrotaMan.Models;
using Microsoft.AspNetCore.Mvc;
using FrotaMan.Services;
using FrotaMan.Repositories;


namespace FrotaMan.Data;

[ApiController]
[Route("api/[controller]")]
public class FleetController : ControllerBase
{
    private FleetContext _fleetContext;

    public FleetController(FleetContext context)
    {
        _fleetContext = context;
    }
    
    [HttpGet]
    public List<VehicleModel> Get([FromServices] IVehicleRepository vehicleRepository)
    {
        return vehicleRepository.List();
    }

    [HttpGet("{vehicleId:int}")]
    public async Task<VehicleModel?> Get([FromRoute] int vehicleId, [FromServices] IVehicleRepository vehicleRepository)
    {
        return await vehicleRepository.Get(vehicleId);
    }

    [HttpPost("vehicle")]
    public IResult createVehicle(
        [FromBody] JsonElement json,
        [FromServices] IJsonPolymorphicService jsonService,
        [FromServices] IVehicleRepository vehicleRepository,
        [FromServices] IUnitOfWork unitOfWork
    )
    {
        VehicleModel? newVehicle;
        string orderedJson = jsonService.PrepareJson<VehicleModel>(json);
        try
        {
            newVehicle = JsonSerializer.Deserialize<VehicleModel>(orderedJson);
        } catch
        {
            newVehicle = null;
        }

        if(newVehicle == null)
        {
            json.TryGetProperty("tankVolume", out var tankVolume);
            if (tankVolume.GetType() == null)
            {
                return Results.BadRequest("Tipo e volume do tanque de combustível não informados");
            }
            int tankVolumeInt = tankVolume.GetInt32();
            if (tankVolumeInt < 2 || tankVolumeInt > 1000 )
            {
                return Results.BadRequest("O volume do tanque deve estar entre 2 e 1000");
            }
            try
            {
                newVehicle = tankVolumeInt switch
                    {
                        >= 2 and <= 30   => JsonSerializer.Deserialize<MotorcycleModel>(json.GetRawText())!,
                        > 30 and <= 100  => JsonSerializer.Deserialize<CarModel>(json.GetRawText())!,
                        > 100 and <= 1000 => JsonSerializer.Deserialize<TruckModel>(json.GetRawText())!,
                        _ => null
                    };
            } catch (JsonException error) when (error.Message.Contains("missing required properties"))
            {
                var match = System.Text.RegularExpressions.Regex.Match(error.Message, @"including:\s+'([^']*)'");
                if (match.Success)
                {
                    string missingField = match.Groups[1].Value;
                    return Results.BadRequest($"Não foi possível criar o veículo pois o campo '{missingField}' está ausente.");
                }
            } catch (Exception error)
            {
                return Results.BadRequest(error.Message);
            }
        }
        if (newVehicle == null)
        {
            return Results.BadRequest("A operação não foi concluída");
        }

        vehicleRepository.Save(newVehicle);
        unitOfWork.Commit();
        
        return Results.Created("api/Fleet/vehicle", newVehicle);
    }
}
