using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using tms_acl_api.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Expose configuration via static accessor (used by legacy static classes)
AppConfiguration.Current = builder.Configuration;

// Add controllers with Newtonsoft.Json support
builder.Services.AddControllers()
    .AddNewtonsoftJson();

// JWT authentication
var jwtKey = builder.Configuration["AppSettings:JWTSecurityKey"] ?? string.Empty;
var jwtIssuer = builder.Configuration["AppSettings:JWTIssuer"];
var jwtAudience = builder.Configuration["AppSettings:JWTAudience"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(jwtKey))
    };
});

builder.Services.AddAuthorization();

// CORS - allow all origins (adjust as needed for production)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
