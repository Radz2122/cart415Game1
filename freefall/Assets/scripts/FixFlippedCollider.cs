using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class FixFlippedCollider : MonoBehaviour
{
    private PolygonCollider2D polygonCollider;
    private SpriteRenderer spriteRenderer;
    private Vector2[] originalPoints; // Store original collider shape

    void Start()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (polygonCollider != null && spriteRenderer != null)
        {
            // Store original collider points (local space)
            originalPoints = (Vector2[])polygonCollider.points.Clone();
            UpdateCollider();
        }
    }

    void UpdateCollider()
    {
        if (polygonCollider == null || originalPoints == null || spriteRenderer == null) return;

        Vector2[] newPoints = new Vector2[originalPoints.Length];

        // Detect if the sprite is flipped
        bool flipX = spriteRenderer.flipX;
        bool flipY = spriteRenderer.flipY; // Only works in Unity 2023+. Older versions may need alternative methods.

        for (int i = 0; i < originalPoints.Length; i++)
        {
            Vector2 point = originalPoints[i];

            // Flip X if flipX is enabled
            if (flipX)
                point.x = -point.x;

            // Flip Y if flipY is enabled
            if (flipY)
                point.y = -point.y;

            newPoints[i] = point;
        }

        polygonCollider.SetPath(0, newPoints); // Apply new collider shape
        polygonCollider.enabled = false; // Force Unity to refresh collider
        polygonCollider.enabled = true;
    }

    // If you flip the object dynamically, call this function
    public void RecalculateCollider()
    {
        UpdateCollider();
    }

    // 🔹 BONUS: Auto-update collider in the Unity Editor when modifying flipX or flipY
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            polygonCollider = GetComponent<PolygonCollider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (polygonCollider != null && spriteRenderer != null)
            {
                UpdateCollider();
                EditorUtility.SetDirty(polygonCollider); // Mark collider as changed
            }
        }
    }
#endif
}
