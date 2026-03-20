using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ClientDefinition", menuName = "Upsijam/Definitions/Client")]
public class ClientDefinition : ScriptableObject
{
    [SerializeField] private string displayName;
    [SerializeField] private GameObject prefab;
    [SerializeField] private int quantityRequested = 1;
    [SerializeField] private List<CriterionRatio> criteriaRatios = new();

    public string DisplayName => displayName;
    public GameObject Prefab => prefab;
    public int QuantityRequested => quantityRequested;
    public IReadOnlyList<CriterionRatio> CriteriaRatios => criteriaRatios;
}
