using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BoxBounds : MonoBehaviour
{
    [SerializeField] private BoxCollider bounds;
    [SerializeField] private float padding = 0.1f;

    private void Reset() => bounds = GetComponent<BoxCollider>();

    private void OnValidate()
    {
        if (bounds == null)
        {
            bounds = GetComponent<BoxCollider>();
        }

        if (padding < 0f)
        {
            padding = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (bounds == null)
        {
            return;
        }

        Vector3 local = transform.InverseTransformPoint(other.transform.position) - bounds.center;
        Vector3 extents = bounds.size * 0.5f;
        Vector3 innerExtents = new Vector3(
            Mathf.Max(0f, extents.x - padding),
            Mathf.Max(0f, extents.y - padding),
            Mathf.Max(0f, extents.z - padding));

        Vector3 clampedLocal = new Vector3(
            Mathf.Clamp(local.x, -innerExtents.x, innerExtents.x),
            Mathf.Clamp(local.y, -innerExtents.y, innerExtents.y),
            Mathf.Clamp(local.z, -innerExtents.z, innerExtents.z));

        other.transform.position = transform.TransformPoint(clampedLocal + bounds.center);

        Rigidbody rb = other.attachedRigidbody;
        if (rb == null)
        {
            return;
        }

        Vector3 overflow = new Vector3(
            Mathf.Abs(local.x) - extents.x,
            Mathf.Abs(local.y) - extents.y,
            Mathf.Abs(local.z) - extents.z);

        Vector3 localNormal;
        if (overflow.x >= overflow.y && overflow.x >= overflow.z)
        {
            localNormal = new Vector3(Mathf.Sign(local.x), 0f, 0f);
        }
        else if (overflow.y >= overflow.x && overflow.y >= overflow.z)
        {
            localNormal = new Vector3(0f, Mathf.Sign(local.y), 0f);
        }
        else
        {
            localNormal = new Vector3(0f, 0f, Mathf.Sign(local.z));
        }

        Vector3 worldNormal = transform.TransformDirection(localNormal).normalized;
        rb.linearVelocity = Vector3.Reflect(rb.linearVelocity, worldNormal);
    }
}
