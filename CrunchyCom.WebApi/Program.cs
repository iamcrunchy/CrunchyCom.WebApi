using System.Text;
using CrunchyCom.Business.Auth;
using CrunchyCom.Business.Services;
using CrunchyCom.Data.Config;
using CrunchyCom.Data.Models;
using CrunchyCom.Data.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Add Serilog configuration (goes right after builder creation so the entire process is logged)
builder.Host.UseSerilog((context, configuration) =>
{
    var mongoDbSettings = context.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
    var connectionString = $"{mongoDbSettings!.ConnectionString}/logs";
    configuration
        .MinimumLevel.Information()
        .WriteTo.MongoDB(
            connectionString,
            "logs",
            LogEventLevel.Warning)
        .WriteTo.File(
            "logs/api-.txt",
            rollingInterval: RollingInterval.Day,
            restrictedToMinimumLevel: LogEventLevel.Information)
        .Enrich.FromLogContext()
        .Enrich.WithProperty("Application", "CrunchyCom.WebApi");
});

// Add services to the container.

// Configure MongoDB settings
var options = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
// Configure MongoDB settings
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddSingleton<MongoDbSettings>(sp =>
    sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);

// Register MongoDB client
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<MongoDbSettings>();
    return new MongoClient(settings.ConnectionString);
});

// Register repositories
builder.Services.AddScoped<IRepository<Post>, PostRepository>();
builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<UserRepository>();

// Register services
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IUserService, UserService>();

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
            .AllowAnyHeader()
            .AllowCredentials();
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
    app.UseSwagger();
    app.UseSwaggerUI();
}

// TODO: Uncomment the following line to enable HTTPS redirection
//app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

// add authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// using top-level statements instead of UseRouting() and UseEndpoints()
app.MapControllers();

app.Run();