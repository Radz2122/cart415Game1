using UnityEngine;

public class HexagonSpawner : MonoBehaviour
{
    public GameObject hexagonPrefab; // Assign in Inspector
    public float spawnRate = 5f; // Time between spawns
    public float xOffset = 4.5f; // Distance from center (adjust for screen width)
    public Transform balloon; // Reference to the balloon

    private float nextSpawnY; // Tracks next spawn position

    void Start()
    {
        nextSpawnY = balloon.position.y - 5f; // Start below balloon
    }

    void Update()
    {
        // Keep spawning as the balloon moves down
        if (balloon.position.y < nextSpawnY)
        {
            SpawnHexagon();
            nextSpawnY -= spawnRate; // Move spawn position down
        }
    }

    void SpawnHexagon()
    {
        float spawnY = balloon.position.y - 10f; // Spawn below balloon
        
        // Left hexagon
        Instantiate(hexagonPrefab, new Vector3(-xOffset, spawnY, 0), Quaternion.identity);

        // Right hexagon
        Instantiate(hexagonPrefab, new Vector3(xOffset, spawnY, 0), Quaternion.identity);
    }
}
