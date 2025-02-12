using UnityEngine;

public class HexagonDestroyer : MonoBehaviour
{
    private float destroyOffset = 10f; // Extra buffer before destruction

    void Update()
    {
        if (transform.position.y > Camera.main.transform.position.y + destroyOffset)
        {
            Destroy(gameObject); // Destroy hexagon when it moves off-screen
        }
    }
}
