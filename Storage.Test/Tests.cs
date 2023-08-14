using FluentAssertions;
using Storage.Entities.Implementations;
using Xunit;
namespace Storage.Test;

public class Tests
{
    [Fact]
    public void AddBox_CreationTest()
    {
        var box = new Box(
            Guid.NewGuid(),
            10,
            10,
            10,
            10,
            DateOnly.FromDateTime(DateTime.Now),
            null);

        box.Width.Should().Be(10);
        box.Volume.Should().Be(1000);
        box.Expiration.Should().Be(DateOnly.FromDateTime(DateTime.Now));
    }

    [Fact]
    public void SetDate_ManufactureDateTest()
    {
        var box = new Box(
            Guid.NewGuid(),
            10,
            10,
            10,
            10,
            DateOnly.FromDateTime(DateTime.Now.AddDays(10)),
            DateOnly.FromDateTime(DateTime.Now));

        box.Expiration.Should().Be(DateOnly.FromDateTime(DateTime.Now).AddDays(Box.ExpirationDays));
        box.ManufacturedDate.Should().Be(DateOnly.FromDateTime(DateTime.Now));
    }

    [Fact]
    public void AddPallet_CreationTest()
    {
        var pallet = new Pallet(
            Guid.NewGuid(),
            10,
            10,
            10);

        pallet.Expiration.Should().Be(default);
        pallet.Volume.Should().Be(1000);
        pallet.Weight.Should().Be(30);
    }
    
    [Fact]
    public void AddBoxesInPallet_CreationTest()
    {
        var box1 = new Box(
            Guid.NewGuid(),
            1,
            1,
            1,
            1,
            DateOnly.FromDateTime(DateTime.Now.AddDays(10)),
            DateOnly.FromDateTime(DateTime.Now));
        var box2 = new Box(
            Guid.NewGuid(),
            1,
            1,
            1,
            1,
            DateOnly.FromDateTime(DateTime.Now.AddDays(10)),
            null);
        var box3 = new Box(
            Guid.NewGuid(),
            1,
            1,
            1,
            1,
            DateOnly.FromDateTime(DateTime.Now.AddDays(10)),
            null);
        
        var pallet = new Pallet(
            Guid.NewGuid(),
            10,
            10,
            10);

        pallet.PutBox(box1);
        pallet.PutBox(box2);
        pallet.PutBox(box3);

        pallet.Volume.Should().Be(1003);
        pallet.Weight.Should().Be(33);
        pallet.Expiration.Should().Be(DateOnly.FromDateTime(DateTime.Now).AddDays(10));
    }

    [Fact]
    public void AddBoxInPallet_BoxNotFitTest()
    {
        var box = new Box(
            Guid.NewGuid(),
            10,
            10,
            10,
            10,
            DateOnly.FromDateTime(DateTime.Now.AddDays(10)),
            null);
        
        var pallet = new Pallet(
            Guid.NewGuid(),
            1,
            1,
            1);

        Action action = () => pallet.PutBox(box);
        action.Should().Throw<ArgumentException>();
    }
    
}