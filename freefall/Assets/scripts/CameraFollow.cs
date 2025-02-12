using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform balloon;
    public float smoothSpeed = 0.2f;

    void FixedUpdate()
    {
        if (balloon != null)
        {
            Vector3 newPosition = new Vector3(transform.position.x, balloon.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, newPosition, smoothSpeed);
        }
    }
}
