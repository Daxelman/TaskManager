namespace TaskManager.Domain.Entities;

/// <summary>
/// Groups tasks under a named bucket (e.g. "Work", "Personal").
/// Day 5 will flesh out the one-to-many relationship here.
/// </summary>
public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    // Navigation property — populated by EF Core via Include()
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}
