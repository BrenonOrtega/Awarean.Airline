using System;

namespace Awarean.Airline.Domain;

public interface IEntity<out T>
{
    T Id { get; }
    DateTime? CreatedAt { get; }
    DateTime? UpdatedAt { get; }
}

public abstract class Entity<T> : IEntity<T> 
{
    public virtual T Id { get; } = default(T);
    public virtual DateTime? CreatedAt { get; protected set; }
    public virtual DateTime? UpdatedAt { get; protected set; }

    public DateTime? HasBeenUpdated() => UpdatedAt = DateTime.UtcNow;
}