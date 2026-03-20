using System;
using UnityEngine;

[Serializable]
public class CriterionValue
{
    [SerializeField] private CriterionType criterion;
    [SerializeField] private int value;

    public CriterionType Criterion => criterion;
    public int Value => value;

    public CriterionValue(CriterionType criterion, int value)
    {
        this.criterion = criterion;
        this.value = value;
    }
}
