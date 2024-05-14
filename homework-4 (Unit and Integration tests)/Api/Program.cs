using Api.Application;
using Api.Interceptors;
using Api.Services;
using FluentValidation;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc(o =>
{
    o.Interceptors.Add<ExceptionInterceptor>();
    o.Interceptors.Add<LoggingInterceptor>();
    o.Interceptors.Add<ValidationInterceptor>();
}).AddJsonTranscoding();
builder.Services.AddGrpcSwagger();
builder.Services.AddSwaggerGen();
builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));
builder.Services.AddRepositories();
builder.Services.AddServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Configure the HTTP request pipeline.
app.MapGrpcService<ProductServiceGrpc>();

app.Run();