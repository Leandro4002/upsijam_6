using System.Collections.Generic;

public class Inventory
{
    private readonly List<InventoryEntry> entries = new();

    public IReadOnlyList<InventoryEntry> Entries => entries;

    public void AddResource(ResourceDefinition resource, int quantity)
    {
        if (resource == null || quantity <= 0)
        {
            return;
        }

        InventoryEntry entry = FindEntry(resource);
        if (entry == null)
        {
            entries.Add(new InventoryEntry(resource, quantity));
            return;
        }

        entry.AddQuantity(quantity);
    }

    public bool RemoveResource(ResourceDefinition resource, int quantity)
    {
        if (resource == null || quantity <= 0)
        {
            return false;
        }

        InventoryEntry entry = FindEntry(resource);
        if (entry == null || entry.Quantity < quantity)
        {
            return false;
        }

        entry.RemoveQuantity(quantity);
        if (entry.Quantity == 0)
        {
            entries.Remove(entry);
        }

        return true;
    }

    public int GetQuantity(ResourceDefinition resource)
    {
        if (resource == null)
        {
            return 0;
        }

        InventoryEntry entry = FindEntry(resource);
        return entry == null ? 0 : entry.Quantity;
    }

    public bool HasResource(ResourceDefinition resource, int quantity = 1)
    {
        if (resource == null || quantity <= 0)
        {
            return false;
        }

        return GetQuantity(resource) >= quantity;
    }

    private InventoryEntry FindEntry(ResourceDefinition resource)
    {
        return entries.Find(entry => entry.Resource == resource);
    }
}
