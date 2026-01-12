using HotelReservations_ML_API;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ML;
using Microsoft.OpenApi.Models;
using System.Runtime.Intrinsics.X86;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPredictionEnginePool<BookingCancellationModel.ModelInput, BookingCancellationModel.ModelOutput>()
    .FromFile("BookingCancellationModel.mlnet");

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Hotel Reservations ML API",
        Description = "ML.NET API for predicting booking cancellations",
        Version = "v1"
    });
});

var app = builder.Build();

app.UseSwagger();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hotel Reservations ML API V1");
    });
}

app.MapPost("/predict",
    async (PredictionEnginePool<BookingCancellationModel.ModelInput, BookingCancellationModel.ModelOutput> predictionEnginePool,
           BookingCancellationModel.ModelInput input) =>
        await Task.FromResult(predictionEnginePool.Predict(input)));

app.Run();