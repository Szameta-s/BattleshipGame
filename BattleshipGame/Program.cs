using Newtonsoft.Json.Serialization;
using BattleshipGame.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IShipRepository, ShipRepository>();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddControllers()
    .AddNewtonsoftJson( options =>
    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });
});

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCors(MyAllowSpecificOrigins);
app.UseCookiePolicy();
app.UseAuthorization();

app.MapControllers();

app.Run();
