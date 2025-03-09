using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.2f; //  How long the shake lasts
    public float shakeMagnitude = 0.2f; //  How strong the shake is

    private Vector3 originalPosition;
    private bool isShaking = false;

    void Awake()
    {
        //  Ensure the camera reference is assigned before the game starts
        originalPosition = transform.position;
    }

  
public IEnumerator Shake()
{
    if (isShaking) yield break; //Prevent multiple shakes from stacking
    isShaking = true;

    float elapsed = 0.0f;
    Vector3 originalPosition = transform.position; //  Save original position

    while (elapsed < shakeDuration)
    {
        //  Stop shaking if the game is over
        if (Time.timeScale == 0)
        {
            transform.position = originalPosition; //  Reset camera position
            isShaking = false;
            yield break; //  Exit the coroutine immediately
        }

        float x = Random.Range(-1f, 1f) * shakeMagnitude; //  Shake only on X-axis
        transform.position = originalPosition + new Vector3(x, 0, 0); //  Y-axis remains fixed

        elapsed += Time.deltaTime;
        yield return null;
    }

    transform.position = originalPosition; //  Reset position after shake
    isShaking = false;
}


}
