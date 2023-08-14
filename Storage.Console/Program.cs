using Spectre.Console;
using Storage.Entities.Implementations;

var pallets = GeneratePallet(5);
var boxes = GenerateBox(100);

pallets = PutBoxes(pallets);

var sortedPallets = pallets
    .GroupBy(p => p.Expiration)
    .OrderBy(date => date.Key)
    .Select(date => date.OrderBy(p => p.Weight))
    .SelectMany(date => date)
    .ToList();

PrintTable(sortedPallets, "1");

var selectedPallets = pallets.Where(p => p.Boxes.Any())
    .OrderByDescending(p => p.Boxes.Max(b => b.Expiration))
    .Take(3)
    .OrderBy(p => p.Volume)
    .ToList();

PrintTable(selectedPallets, "2");

List<Box> GenerateBox(int count)
{
    List<Box> boxList = new List<Box>();
    var x = new Random();
    for (var i = 0; i < count; i++)
    {
        var box = new Box(
            Guid.NewGuid(),
            x.Next(1, 100),
            x.Next(1, 100),
            x.Next(1, 100),
            x.Next(1, 100),
            DateOnly.FromDateTime(DateTime.Now).AddDays(x.Next(20, 50)),
            DateOnly.FromDateTime(DateTime.Now).AddDays(x.Next(1, 20))
        );
        
        boxList.Add(box);
    }

    return boxList;
}

List<Pallet> GeneratePallet(int count)
{
    List<Pallet> palletsList = new List<Pallet>();
    var x = new Random();
    for (var i = 0; i < count; i++)
    {
        var pallet = new Pallet(
            Guid.NewGuid(),
            x.Next(10, 100),
            x.Next(10, 100),
            x.Next(10, 100)
        );
        palletsList.Add(pallet);
    }

    return palletsList;
}

List<Pallet> PutBoxes(List<Pallet> pallets)
{
    const int batchSize = 20;

    var groupedBoxes = boxes.Select((box, index) => new { box, index })
        .GroupBy(x => x.index / batchSize, x => x.box)
        .Select(g => g.ToList())
        .ToList();

    int palletIndex = 0;
    foreach (var group in groupedBoxes)
    {
        foreach (var box in group)
        {
            try
            {
                pallets[palletIndex].PutBox(box);
            }
            catch (Exception)
            {
            }
        }

        palletIndex++;
        if (palletIndex >= pallets.Count)
        {
            break;
        }
    }

    return pallets;
}

void PrintTable(ICollection<Pallet> pallets, string name)
{
    var table = new Table();
    table.Title(name);
    table.AddColumn("Pallet");
    table.AddColumn("Box");

    var previousPalletId = Guid.Empty;
    foreach (var p in pallets)
    {
        if (p.Id != previousPalletId)
        {
            table.AddRow($"[green]Id[/]: {p.Id}\n[aqua]Expiration date[/]: {p.Expiration}\nVolume: {p.Volume}", $"[darkcyan]Id[/]: {p.Boxes.First().Id}\nWeight: {p.Boxes.First().Weight}");
            previousPalletId = p.Id;
        }
        else
        {
            table.AddRow("", $"[darkcyan]Id[/]: {p.Boxes.First().Id}\nWeight: {p.Boxes.First().Weight}");
        }

        foreach (var b in p.Boxes.Skip(1))
        {
            table.AddRow("", $"[darkcyan]Id[/]: {p.Boxes.First().Id}\nWeight: {p.Boxes.First().Weight}");
        }
    }
    
    AnsiConsole.Write(table);
}