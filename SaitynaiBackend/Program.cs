using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaitynaiBackend.Data;
using SaitynaiBackend.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SaitynaiBackend.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(o =>
{
    o.Filters.Add<ValidationFilter>();
});
builder.Services.AddScoped<AuthDbSeeder>();
builder.Services.AddTransient<JwtService>();
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


builder.Services.AddIdentity<StoreUser, IdentityRole>().AddEntityFrameworkStores<StoreDbContext>().AddDefaultTokenProviders();
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt =>
{
    opt.MapInboundClaims = false;
    opt.TokenValidationParameters.ValidAudience = builder.Configuration["JWT:ValidAudience"];
    opt.TokenValidationParameters.ValidIssuer = builder.Configuration["JWT:ValidIssuer"];
    opt.TokenValidationParameters.IssuerSigningKey =
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]));
});
builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyMethod()
               .AllowAnyHeader()
               .SetIsOriginAllowed(origin => true)
               .AllowAnyOrigin();
    });
});
// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors("CorsPolicy");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
    dataContext.Database.Migrate();
    var dbSeeder = scope.ServiceProvider.GetRequiredService<AuthDbSeeder>();
    await dbSeeder.SeedAsync();
}
app.Run();