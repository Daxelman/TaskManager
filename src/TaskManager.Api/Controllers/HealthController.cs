using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Infrastructure.Persistence;

namespace TaskManager.Api.Controllers;

/// <summary>
/// Day 1 smoke-test: hit GET /health to confirm the API starts and EF Core
/// can open a connection to the SQLite database.
/// </summary>
[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly ILogger<HealthController> _logger;

    public HealthController(AppDbContext db, ILogger<HealthController> logger)
    {
        _db = db;
        _logger = logger;
    }

    /// <summary>Returns 200 + DB stats when everything is wired up correctly.</summary>
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken ct)
    {
        try
        {
            // CanConnectAsync opens a real connection — if migrations haven't run
            // or the file path is wrong, this throws immediately.
            var canConnect = await _db.Database.CanConnectAsync(ct);

            var taskCount = await _db.Tasks.CountAsync(ct);
            var categoryCount = await _db.Categories.CountAsync(ct);

            _logger.LogInformation(
                "Health check passed. Tasks: {Tasks}, Categories: {Categories}",
                taskCount, categoryCount);

            return Ok(new
            {
                status = "healthy",
                database = new
                {
                    provider = _db.Database.ProviderName,
                    canConnect,
                    pendingMigrations = await _db.Database.GetPendingMigrationsAsync(ct),
                    appliedMigrations = await _db.Database.GetAppliedMigrationsAsync(ct)
                },
                counts = new { taskCount, categoryCount },
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Health check failed");
            return StatusCode(503, new { status = "unhealthy", error = ex.Message });
        }
    }
}
