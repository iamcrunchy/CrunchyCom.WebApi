using CrunchyCom.Data.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using CrunchyCom.Business.Auth;
using CrunchyCom.Business.Services;
using MongoDB.Driver;
using Serilog;
using Serilog.Events;
using MongoDB;

var builder = WebApplication.CreateBuilder(args);

// Add Serilog configuration (goes right after builder creation so the entire process is logged)
builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .MinimumLevel.Information()
        .WriteTo.MongoDB(
            builder.Configuration["MongoDbSettings:ConnectionString"]!,
            "logs",
            restrictedToMinimumLevel: LogEventLevel.Warning)
        .WriteTo.File(
            "logs/api-.txt",
            rollingInterval: RollingInterval.Day,
            restrictedToMinimumLevel: LogEventLevel.Information)
        .Enrich.FromLogContext()
        .Enrich.WithProperty("Application", "CrunchyCom.WebApi");
});

// Add services to the container.

// Configure MongoDB settings
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddScoped<IPostService, PostService>();

// Configure JWT authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings")
    .Get<JwtSettings>();

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
            ValidIssuer = jwtSettings!.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
        };
    });
    // .AddGoogle(options =>
    // {
    //     options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
    //     options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
    // });
    
    
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Your Angular frontend URL
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});



// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// TODO: Uncomment the following line to enable HTTPS redirection
//app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowFrontend");

// add authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// using top-level statements instead of UseRouting() and UseEndpoints()
app.MapControllers();

app.Run();