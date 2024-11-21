using Ideas.Models;
using Microsoft.EntityFrameworkCore;
using Ideas.Modules;

var builder = WebApplication.CreateBuilder(args);

// Configuration
var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("DefaultConnection");

var token = configuration.GetValue<string>("SmsApi:ApiKey");
builder.Services.AddSingleton(token);

builder.Services.AddDbContext<ProductReviewContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<SmsSender>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options =>
{
    options.AllowAnyOrigin();
    options.AllowAnyMethod();
    options.AllowAnyHeader();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();