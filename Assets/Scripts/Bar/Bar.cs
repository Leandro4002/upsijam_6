using UnityEngine;

public class Bar : MonoBehaviour
{
    [SerializeField] private Transform initSpawnPoint;
    [SerializeField] private float spawnRadius;
    [SerializeField] private GameObject[] prefabsToSpawn;
    [SerializeField] private Camera camera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Rotate camera 90 degrees around the Y-axis
        //Camera.main.transform.Rotate(0, 180, 0);


        // Spawns random number of game objects between 1 and 5 at the spawn position
        if (prefabsToSpawn == null || prefabsToSpawn.Length == 0)
        {
            return;
        }

        Vector3 center = initSpawnPoint != null ? initSpawnPoint.position : transform.position;

        int amount = Random.Range(30, 60);

        for (int i = 0; i < amount; i++)
        {
            GameObject randomPrefab = prefabsToSpawn[Random.Range(0, prefabsToSpawn.Length)];

            if (randomPrefab == null)
            {
                continue;
            }

            // Randomize spawn position within the spawn radius
            Vector3 spawnPosition = center + Random.insideUnitSphere * spawnRadius;
            Quaternion spawnRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            Instantiate(randomPrefab, spawnPosition, spawnRotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
