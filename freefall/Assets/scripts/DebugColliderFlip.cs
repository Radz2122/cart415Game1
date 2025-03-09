using UnityEngine;

public class DebugColliderFlip : MonoBehaviour
{
    void Update()
    {
        Debug.Log(gameObject.name + " - Local Scale: " + transform.localScale + " | Lossy Scale: " + transform.lossyScale);
    }
}
