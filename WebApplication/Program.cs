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


app.MapGet("/test-endpoint",
        async (ISchoolRepository schoolRepo, IStaffRepository staffRepo, IConnectionMultiplexer redis) =>
        {
            var schools = schoolRepo.GetAll();
            var staff = staffRepo.GetAll();
            var redisInfo = redis.GetStatus();

            return Results.Ok(new
            {
                Schools = schools,
                Staff = staff,
                RedisInfo = redisInfo
            });
        })
    .WithName("TestEndpoint");

app.MapControllers();

app.Run();

void ConfigureDatabase(WebApplicationBuilder builder)
{
    builder.Services.AddDbContext<DatabaseContext>(options =>
        options.UseNpgsql(builder.Configuration["Postgres:ConnectionString"]));

    builder.Services.AddScoped<ISchoolRepository, SchoolRepository>();
    builder.Services.AddScoped<IStaffRepository, StaffRepository>();
}

void ConfigureCache(WebApplicationBuilder builder)
{
    builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
        ConnectionMultiplexer.Connect(builder.Configuration["Redis:ConnectionString"]));
    builder.Services.AddSingleton<IRedisAdapter, RedisAdapter>();

    builder.Services.AddMemoryCache();
}

void ConfigureServices(WebApplicationBuilder builder)
{
    //builder.Services.AddTransient<Service<Staff>>(); blocked with the use of RepositoryBase
}
