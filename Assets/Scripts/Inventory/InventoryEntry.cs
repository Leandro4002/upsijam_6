using System;
using UnityEngine;

[Serializable]
public class InventoryEntry
{
    [SerializeField] private ResourceDefinition resource;
    [SerializeField] private int quantity;

    public ResourceDefinition Resource => resource;
    public int Quantity => quantity;

    public InventoryEntry(ResourceDefinition resource, int quantity)
    {
        this.resource = resource;
        this.quantity = quantity;
    }

    public void AddQuantity(int amount)
    {
        quantity += amount;
    }

    public void RemoveQuantity(int amount)
    {
        quantity -= amount;
    }
}
