using UnityEngine;

public class ConveyorItem : MonoBehaviour
{
    [SerializeField] private ResourceDefinition resource;
    [SerializeField] private Vector3 cleanupOrigin;
    [SerializeField, Min(0f)] private float maxDistanceFromOrigin = 30f;

    public ResourceDefinition Resource => resource;

    public void Initialize(ResourceDefinition definition, Vector3 origin, float maxDistance)
    {
        resource = definition;
        cleanupOrigin = origin;
        maxDistanceFromOrigin = Mathf.Max(0f, maxDistance);
    }

    private void Update()
    {
        if (maxDistanceFromOrigin <= 0f)
        {
            return;
        }

        if ((transform.position - cleanupOrigin).sqrMagnitude > maxDistanceFromOrigin * maxDistanceFromOrigin)
        {
            Destroy(gameObject);
        }
    }
}
