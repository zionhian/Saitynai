using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaitynaiBackend.Data;
using SaitynaiBackend.Data.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(o =>
{
    o.Filters.Add<ValidationFilter>();
});

// Register DbContext and PostgreSQL connection
builder.Services.AddDbContext<StoreDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

// Add FluentValidation services and validators
builder.Services.AddFluentValidationAutoValidation()
    .AddValidatorsFromAssemblyContaining<GamePostDtoValidator>()
    .AddValidatorsFromAssemblyContaining<GamePutDtoValidator>()
    .AddValidatorsFromAssemblyContaining<PublisherPostDtoValidator>()
    .AddValidatorsFromAssemblyContaining<PublisherPutDtoValidator>()
    .AddValidatorsFromAssemblyContaining<ReviewPostDtoValidator>()
    .AddValidatorsFromAssemblyContaining<ReviewPutDtoValidator>();

// Add Swagger for API documentation
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
app.UseAuthorization();
app.MapControllers();
app.Run();