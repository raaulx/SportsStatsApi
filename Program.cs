using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SportsStatsApi.Data;
using SportsStatsApi.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// ─── Database Configuration ─────────────────────────────────────────────────
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Data Source=SportsStats.db"));

// ─── Services Registration ──────────────────────────────────────────────────
builder.Services.AddScoped<IFighterService, FighterService>();

// ─── Controllers ────────────────────────────────────────────────────────────
builder.Services.AddControllers();

// ─── Swagger / OpenAPI Configuration ────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "🥊 Sports Stats API",
        Description = "A RESTful API for UFC fighter statistics, comparisons, and division rankings. " +
                      "Built with ASP.NET Core 8, Entity Framework Core, and SQLite.",
        Contact = new OpenApiContact
        {
            Name = "Arturo",
            Url = new Uri("https://github.com/arturo")
        },
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });

    // Include XML comments in Swagger documentation
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

// ─── CORS Configuration ────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// ─── Ensure Database is Created and Seeded ──────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// ─── HTTP Request Pipeline ──────────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Sports Stats API v1");
        options.RoutePrefix = string.Empty; // Swagger at root URL
        options.DocumentTitle = "🥊 Sports Stats API - Swagger";
    });
}

app.UseCors();
app.UseAuthorization();
app.MapControllers();

// ─── Welcome Message ────────────────────────────────────────────────────────
app.Logger.LogInformation("🥊 Sports Stats API is running!");
app.Logger.LogInformation("📖 Swagger UI available at: http://localhost:{Port}/",
    app.Configuration["ASPNETCORE_URLS"]?.Split(':').LastOrDefault() ?? "5000");

app.Run();
