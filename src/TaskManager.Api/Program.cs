using Microsoft.EntityFrameworkCore;
using TaskManager.Infrastructure;
using TaskManager.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// ── Services ──────────────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Task Manager API",
        Version = "v1",
        Description = "A .NET Core + EF Core practice project built day by day."
    });
});

// Infrastructure layer registers AppDbContext + future repos/services.
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// ── Auto-migrate on startup (dev only) ────────────────────────────────────
// In production you'd run migrations as a separate deployment step.
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

// ── Middleware pipeline ────────────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

// Expose Program for WebApplicationFactory in integration tests (Day 13).
public partial class Program { }
