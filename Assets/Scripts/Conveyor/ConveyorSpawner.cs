using UnityEngine;

public class ConveyorSpawner : MonoBehaviour
{
    [SerializeField] private ResourceCatalog catalog;
    [SerializeField] private Transform spawnPoint;
    [SerializeField, Min(0.1f)] private float spawnInterval = 2f;
    [SerializeField, Min(0f)] private float cleanupDistance = 30f;
    [SerializeField, Min(0f)] private float spawnBlockRadius = 0.5f;
    [SerializeField] private LayerMask spawnBlockMask = ~0;

    private float nextSpawnTime;

    private void Update()
    {
        if (Time.time < nextSpawnTime)
        {
            return;
        }

        nextSpawnTime = Time.time + spawnInterval;
        TrySpawn();
    }

    private void TrySpawn()
    {
        if (catalog == null)
        {
            Debug.LogWarning("ConveyorSpawner is missing a ResourceCatalog.", this);
            return;
        }

        if (spawnPoint == null)
        {
            Debug.LogWarning("ConveyorSpawner is missing a spawn point.", this);
            return;
        }

        if (!TrySelectResource(out ResourceDefinition selectedResource))
        {
            Debug.LogWarning("ConveyorSpawner could not find a valid resource to spawn in the assigned catalog.", this);
            return;
        }

        if (selectedResource.Prefab == null)
        {
            Debug.LogWarning($"ConveyorSpawner skipped '{selectedResource.DisplayName}' because it has no prefab assigned.", this);
            return;
        }

        if (spawnBlockRadius > 0f && Physics.CheckSphere(spawnPoint.position, spawnBlockRadius, spawnBlockMask, QueryTriggerInteraction.Ignore))
        {
            return;
        }

        GameObject instance = Instantiate(selectedResource.Prefab, spawnPoint.position, spawnPoint.rotation);

        Rigidbody instanceRigidbody = instance.GetComponent<Rigidbody>();
        if (instanceRigidbody == null)
        {
            instanceRigidbody = instance.AddComponent<Rigidbody>();
            Debug.LogWarning($"Spawned prefab '{instance.name}' was missing a Rigidbody, so one was added automatically.", instance);
        }

        ConveyorItem conveyorItem = instance.GetComponent<ConveyorItem>();
        if (conveyorItem == null)
        {
            conveyorItem = instance.AddComponent<ConveyorItem>();
        }

        conveyorItem.Initialize(selectedResource, spawnPoint.position, cleanupDistance);
    }

    private bool TrySelectResource(out ResourceDefinition resource)
    {
        resource = null;

        if (catalog.Resources == null || catalog.Resources.Count == 0)
        {
            return false;
        }

        float totalWeight = 0f;
        for (int index = 0; index < catalog.Resources.Count; index++)
        {
            WeightedResourceEntry entry = catalog.Resources[index];
            if (entry == null || entry.Resource == null || entry.Weight <= 0f)
            {
                continue;
            }

            totalWeight += entry.Weight;
        }

        if (totalWeight <= 0f)
        {
            return false;
        }

        float roll = Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;

        for (int index = 0; index < catalog.Resources.Count; index++)
        {
            WeightedResourceEntry entry = catalog.Resources[index];
            if (entry == null || entry.Resource == null || entry.Weight <= 0f)
            {
                continue;
            }

            cumulativeWeight += entry.Weight;
            if (roll <= cumulativeWeight)
            {
                resource = entry.Resource;
                return true;
            }
        }

        return false;
    }
}
