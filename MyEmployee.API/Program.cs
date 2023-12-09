using MyEmployee.API.Services;
using MyEmployee.Domain.AggregateModels.EmployeeAggregates;
using MyEmployee.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

//
builder.Services.AddScoped<IEmployeeRepository, FakeEmployeeRepository>();

builder.Services.AddHostedService<EmploeeUpdaterHostedService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcService<WorkerIntegrationService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
