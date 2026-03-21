using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceCatalog", menuName = "Upsijam/Definitions/Resource Catalog")]
public class ResourceCatalog : ScriptableObject
{
    [SerializeField] private List<WeightedResourceEntry> resources = new();

    public IReadOnlyList<WeightedResourceEntry> Resources => resources;
}
