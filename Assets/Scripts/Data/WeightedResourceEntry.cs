using System;
using UnityEngine;

[Serializable]
public class WeightedResourceEntry
{
    [SerializeField] private ResourceDefinition resource;
    [SerializeField, Min(0f)] private float weight = 1f;

    public ResourceDefinition Resource => resource;
    public float Weight => weight;
}
