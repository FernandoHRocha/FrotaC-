using FrotaMan.Data;
using FrotaMan.Repositories;
using FrotaMan.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionStr = builder.Configuration.GetConnectionString("FleetConnection");

builder.Services.AddDbContext<FleetContext>(
    options => options.UseMySql(connectionStr, ServerVersion.AutoDetect(connectionStr))
);
builder.Services.AddTransient<IVehicleRepository, VehicleRepository>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IJsonPolymorphicService, JsonPolymorphicService>();


builder.Services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.AllowOutOfOrderMetadataProperties = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();