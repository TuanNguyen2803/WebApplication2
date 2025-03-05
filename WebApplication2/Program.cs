using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Hangfire.Mongo.Migration.Strategies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using WebApplication2.AppDataContext;
using WebApplication2.Service;
using Microsoft.AspNetCore.Hosting.Builder;
using WebApplication2.Configuration;
using HangfireBasicAuthenticationFilter;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<SystemJobSettings>(builder.Configuration.GetSection(nameof(SystemJobSettings)));
// L?y chu?i k?t n?i t? appsettings.json
var connectionString = builder.Configuration.GetConnectionString("MongoDb");

// Thêm Hangfire
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

// Thêm Hangfire Server
builder.Services.AddHangfireServer();

// Thêm MongoDB Client
builder.Services.AddSingleton<IMongoClient>(new MongoClient(connectionString));

// Thêm AppDbContext và NumberUpdateService
builder.Services.AddScoped<AppDbContext>();
builder.Services.AddScoped<NumberInitializationService>();
//builder.Services.AddScoped<NumberService>();
builder.Services.AddScoped<ISystemJobSettings,SystemJobSettings>();

//ap
builder.Services.Configure<SystemJobSettings>(builder.Configuration.GetSection("SystemJobSettings"));
builder.Services.Configure<ISystemJobSettings>(builder.Configuration.GetSection("SystemJobSettings"));
//builder.Services.AddScoped<ISystemJobSettings>(provider => provider.GetRequiredService<Microsoft.Extensions.Options.IOptions<SystemJobSettings>>().Value);
// Thêm Controllers
builder.Services.AddControllers();

// Thêm Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// C?u hình HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Thêm Hangfire Dashboard
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] {
        new HangfireCustomBasicAuthenticationFilter
        {
            User = builder.Configuration.GetSection("HangfireSettings:UserName").Value,
            Pass = builder.Configuration.GetSection("HangfireSettings:Password").Value
        }
    }
});

// Thêm Hangfire Server
app.UseHangfireServer();

app.MapControllers();

// Thêm Recurring Job (ví d?: ch?y m?i phút)
//RecurringJob.AddOrUpdate<NumberUpdateService>("update-number", service => service.UpdateNumberValue(),cronExpression);

app.Run();