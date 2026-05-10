using TaskStatus = TaskManager.Domain.Enums.TaskStatus;

namespace TaskManager.Domain.Entities;

/// <summary>
/// Core aggregate of the system. Intentionally kept lean on Day 1 —
/// audit fields, soft-delete, and tags arrive in later days.
/// </summary>
public class TaskItem : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TaskStatus Status { get; set; } = TaskStatus.Todo;
    public DateTime? DueDate { get; set; }
    public bool IsCompleted { get; set; }

    // FK + navigation for the Category relationship (Day 5)
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
}
