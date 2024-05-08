using DataAccess;
using Domain;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

IProductHistoryRepository productHistoryRepository = new ProductHistoryRepository();
IAdsCalc adsCalc = new AdsCalc();
IPredictionCalc predictionCalc = new PredictionCalc();
IDemandCalc demandCalc = new DemandCalc();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/products/{id}", (int id) =>
{
    var productHistory = productHistoryRepository.Get(id);
    return new
    {
        productId = id,
        rowsCount = productHistory.SalesRows.Count,
        salesHistory = productHistory.SalesRows
    };
});

app.MapGet("/products/{id}/ads", (int id) =>
{
    var productHistory = productHistoryRepository.Get(id);
    var ads = adsCalc.Calculate(productHistory);
    return new
    {
        productId = id,
        ads = ads
    };
});

app.MapGet("/products/{id}/prediction", (int id, [FromQuery] int days) =>
{
    var productHistory = productHistoryRepository.Get(id);
    var ads = adsCalc.Calculate(productHistory);
    var prediction = predictionCalc.Calculate(productHistory, ads, days);
    return new
    {
        productId = id,
        ads = ads,
        prediction = prediction
    };
});

app.MapGet("/products/{id}/demand", (int id, [FromQuery] int days) =>
{
    var productHistory = productHistoryRepository.Get(id);
    var ads = adsCalc.Calculate(productHistory);
    var prediction = predictionCalc.Calculate(productHistory, ads, days);
    
    var lastRow = productHistory.SalesRows.MaxBy(s => s.SaleDate);
    var quantityInStock = lastRow.StockCount - lastRow.SalesCount;
    var demand = demandCalc.Calculate(prediction, quantityInStock);
    return new
    {
        productId = id,
        ads = ads,
        prediction = prediction,
        quantityInStock = quantityInStock,
        demand = demand
    };
});

app.Run();