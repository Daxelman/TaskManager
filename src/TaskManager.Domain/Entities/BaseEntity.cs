namespace TaskManager.Domain.Entities;

/// <summary>
/// Base class for all domain entities. Provides a strongly-typed int Id
/// so every entity has a consistent primary key shape.
/// </summary>
public abstract class BaseEntity
{
    public int Id { get; set; }
}
