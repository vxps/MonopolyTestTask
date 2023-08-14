namespace Storage.Entities.Abstractions;

public abstract class Item
{
    protected Item(Guid id, float width, float height, float depth)
    {
        Id = id;
        Width = width;
        Height = height;
        Depth = depth;
    }
    
    public Guid Id { get; }
    
    public float Width { get; }

    public float Height { get; }
    
    public float Depth { get; }
    
    public virtual float Weight { get; }
    
    public virtual DateOnly Expiration { get; }

    public virtual float Volume { get; }
}