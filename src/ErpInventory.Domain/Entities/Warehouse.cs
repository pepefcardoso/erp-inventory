namespace ErpInventory.Domain.Entities;

public class Warehouse
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Location { get; private set; } = null!;

    private Warehouse() { }

    public Warehouse(string name, string location)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));
        if (string.IsNullOrWhiteSpace(location))
            throw new ArgumentException("Location is required.", nameof(location));

        Id = Guid.NewGuid();
        Name = name;
        Location = location;
    }
}