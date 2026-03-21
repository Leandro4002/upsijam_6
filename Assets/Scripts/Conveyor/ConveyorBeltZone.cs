using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ConveyorBeltZone : MonoBehaviour
{
    [SerializeField] private Vector3 localDirection = Vector3.forward;
    [SerializeField, Min(0f)] private float acceleration = 5f;

    private void Reset()
    {
        Collider beltCollider = GetComponent<Collider>();
        beltCollider.isTrigger = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (acceleration <= 0f)
        {
            return;
        }

        Rigidbody attachedRigidbody = other.attachedRigidbody;
        if (attachedRigidbody == null || attachedRigidbody.isKinematic)
        {
            return;
        }

        Vector3 direction = localDirection.sqrMagnitude > 0f ? localDirection.normalized : Vector3.forward;
        Vector3 worldDirection = transform.TransformDirection(direction);
        attachedRigidbody.AddForce(worldDirection * acceleration, ForceMode.Acceleration);
    }
}
