using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceDefinition", menuName = "Upsijam/Definitions/Resource")]
public class ResourceDefinition : ScriptableObject
{
    [SerializeField] private string displayName;
    [SerializeField] private GameObject prefab;
    [SerializeField] private int size = 1;
    [SerializeField] private List<CriterionValue> criteriaValues = new();

    public string DisplayName => displayName;
    public GameObject Prefab => prefab;
    public int Size => size;
    public IReadOnlyList<CriterionValue> CriteriaValues => criteriaValues;
}
