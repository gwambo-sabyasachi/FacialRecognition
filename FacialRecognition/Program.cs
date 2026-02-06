using FacialRecognition.Infrastructure.Repositories;
using FacialRecognition.Domain.Interfaces;
using FacialRecognition.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using FacialRecognition.Application.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<FacialRecognitionDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IFacialRecognition, FacialRecognitionRepository>();
builder.Services.AddScoped<FacialRecognitionService>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<ITransactionService, TransactionServiceServiceRepository>();
builder.Services.AddScoped<TransactionServiceService>();
builder.Services.AddScoped<IFacialRecognitionDevice, FacialRecognitionDeviceRepository>();
builder.Services.AddScoped<IAttendanceRepository, AttendanceRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
