using Storage.Entities.Abstractions;

namespace Storage.Entities.Implementations;

public class Box : Item
{
    public const int ExpirationDays = 100;
    
    public Box(Guid id, float width, float height, float depth, float weight, DateOnly expiration, DateOnly? manufactureDate)
        : base(id, width, height, depth)
    {
        Weight = weight;
        Volume = width * height * depth;
        Expiration = manufactureDate?.AddDays(ExpirationDays) ?? expiration;
        ManufacturedDate = manufactureDate;
    }
    
    public override float Weight { get; }

    public override float Volume { get; }

    public override DateOnly Expiration { get; }

    public DateOnly? ManufacturedDate { get; }
}