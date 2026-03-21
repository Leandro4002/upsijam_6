using UnityEngine;

public class SphereBounds : MonoBehaviour
{
    [SerializeField] private SphereCollider bounds;
    [SerializeField] private float padding = 0.1f;

    private void Reset() => bounds = GetComponent<SphereCollider>();

    private void OnTriggerExit(Collider other)
    {
        Vector3 center = transform.TransformPoint(bounds.center);
        float radius = bounds.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);

        Vector3 dir = (other.transform.position - center).normalized;
        other.transform.position = center + dir * (radius - padding);

        if (other.attachedRigidbody != null)
        {
            other.attachedRigidbody.linearVelocity =
                Vector3.Reflect(other.attachedRigidbody.linearVelocity, -dir);
        }
    }
}