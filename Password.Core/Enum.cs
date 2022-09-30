namespace Password.Core;

public abstract class Enum<T> : IEquatable<Enum<T>>
{
    public T Id { get; }
    public string Name { get; }

    public Enum(T id, string name)
    {
        Id = id;
        Name = name;
    }

    public bool Equals(Enum<T>? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return EqualityComparer<T>.Default.Equals(Id, other.Id) && Name == other.Name;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Enum<T>) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name);
    }
}
