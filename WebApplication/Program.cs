using DevSchoolCache;
using Microsoft.EntityFrameworkCore;
using Model;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

ConfigureDatabase(builder);
ConfigureCache(builder);
ConfigureServices(builder);
    
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    dbContext.Database.Migrate();
}

app.Run();

void ConfigureDatabase(WebApplicationBuilder builder)
{
    builder.Services.AddDbContext<DatabaseContext>(options =>
        options.UseNpgsql(builder.Configuration["Postgres:ConnectionString"]));

    builder.Services.AddScoped<IRepository<School>, SchoolRepository>();
    builder.Services.AddScoped<IRepository<Staff>, StaffRepository>();
}

void ConfigureCache(WebApplicationBuilder builder)
{
    builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
        ConnectionMultiplexer.Connect(builder.Configuration["Redis:ConnectionString"]));
    builder.Services.AddSingleton<IRedisAdapter, RedisAdapter>();
    builder.Services.AddSingleton<ICacheWrapper, MemoryCacheWrapper>();

    builder.Services.AddMemoryCache();
}

void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.AddTransient(typeof(IHybridCacheService<>), typeof(HybridCacheService<>));
}