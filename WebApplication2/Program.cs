using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Hangfire.Mongo.Migration.Strategies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using WebApplication2.AppDataContext;
using WebApplication2.Service;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("MongoDb");

// Th�m Hangfire
builder.Services.AddHangfire(configuration => configuration
    .UseMongoStorage(builder.Configuration.GetConnectionString("MongoDB"),
        "hangfireDb",
        new MongoStorageOptions
        {
            MigrationOptions = new MongoMigrationOptions
            {
                MigrationStrategy = new MigrateMongoMigrationStrategy(),
                BackupStrategy = new CollectionMongoBackupStrategy()
            }
        }));

// Th�m Hangfire Server
builder.Services.AddHangfireServer();

// Th�m MongoDB Client
builder.Services.AddSingleton<IMongoClient>(new MongoClient(connectionString));

// Th�m AppDbContext v� NumberUpdateService
builder.Services.AddScoped<AppDbContext>();
//builder.Services.AddScoped<NumberUpdateService>();
builder.Services.AddScoped<NumberInitializationService>();
//builder.Services.AddScoped<ChunkProcessingService>();

// Th�m Controllers
builder.Services.AddControllers();

// Th�m Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddAuthentication("Basic")
//    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", null);

builder.Services.AddAuthorization();

var app = builder.Build();

// C?u h�nh HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Th�m Hangfire Dashboard
app.UseHangfireDashboard();

app.UseHangfireServer();

app.MapControllers();
app.Run();