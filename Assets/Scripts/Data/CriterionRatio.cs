using System;
using UnityEngine;

[Serializable]
public class CriterionRatio
{
    [SerializeField] private CriterionType criterion;
    [SerializeField] private float ratio;

    public CriterionType Criterion => criterion;
    public float Ratio => ratio;

    public CriterionRatio(CriterionType criterion, float ratio)
    {
        this.criterion = criterion;
        this.ratio = ratio;
    }
}
