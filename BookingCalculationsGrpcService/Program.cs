using BookingCalculationsGrpcService.Services;
using Microsoft.EntityFrameworkCore;
using Ciciovan_Bogdan_Ionut_HotelReservations.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddGrpc();

// Add DbContext
builder.Services.AddDbContext<HotelReservationsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HotelReservationsDbContext")));

var app = builder.Build();

// Configure the HTTP request pipeline
app.MapGrpcService<BookingCalculationsService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();