using BusinessLogicLayer.Implementations;
using BusinessLogicLayer.Interfaces;
using Common.Entities;
using Common.Validators;
using DataAccessLayer.Implementations;
using DataAccessLayer.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddLogging();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ProviderContext>(options =>
{
    options.UseSqlServer(Environment.GetEnvironmentVariable("ConnectionString")?.Trim('\"')??throw new Exception("There is no connection string for the database"));
});

builder.Services.AddScoped<IDALRepository<Customer>, GenericDALRepository<Customer>>();
builder.Services.AddScoped<IProviderBusinessLogic<Customer>, ProviderCustomerBusinessLogic>();
builder.Services.AddScoped<IValidator<Customer>, CustomerValidator>();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();