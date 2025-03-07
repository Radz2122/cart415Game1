using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.1f;
    public float shakeMagnitude = 0.1f;
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position;
    }

    public void Shake()
    {
        StartCoroutine(ShakeRoutine());
    }

    IEnumerator ShakeRoutine()
    {
        float elapsedTime = 0f;
        while (elapsedTime < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;
            
            transform.position = originalPosition + new Vector3(x, y, 0);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition; // Reset camera position
    }
}
