using Storage.Entities.Abstractions;

namespace Storage.Entities.Implementations;

public class Pallet : Item
{
    private const int PalletWeight = 30;
    private readonly List<Box> _boxes;
    
    public Pallet(Guid id, float width, float height, float depth)
        : base(id, width, height, depth)
    {
        _boxes = new List<Box>();
    }
    
    public IReadOnlyList<Box> Boxes => _boxes;

    public override float Weight
        => _boxes.Sum(box => box.Weight) + PalletWeight;

    public override float Volume
        => _boxes.Sum(box => box.Volume) + Width * Height * Depth;

    public override DateOnly Expiration
        => _boxes.MinBy(box => box.Expiration)?.Expiration ?? default;

    private bool CheckBox(Box box)
        => box.Width > Width || box.Depth > Height;

    public Box PutBox(Box box)
    {
        ArgumentNullException.ThrowIfNull(box);

        if (CheckBox(box))
            throw new ArgumentException($"the box: {box.Id} do not fit this pallet: {Id}");

        _boxes.Add(box);
        return box;
    }

    public void RemoveBox(Box box)
        => _boxes.Remove(box);

}