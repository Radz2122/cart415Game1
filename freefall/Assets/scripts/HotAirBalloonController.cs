using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotAirBalloonController : MonoBehaviour
{
    public float gravityForce = 3.5f; // Gravity to bring it back down
    public float moveSpeed = 3f; // Speed for left/right movement
    public float jumpForce = 12f; // Quick reaction jump
    public float fuel = 100f; // Max fuel
    public float fuelConsumption = 10f; // Fuel cost per jump
    public float maxAscentSpeed = 2.5f; // Prevents floating too long
    public float maxFallSpeed = -5f; // Allows faster descent

    public float swayAmount = 3f; // Sway effect
    public float swaySpeed = 2f; // Sway speed

    public Slider fuelBar; // Reference to UI Fuel Bar

    private Rigidbody2D rb;
    private float swayTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // We handle gravity manually

        // Ensure fuel bar starts full
        if (fuelBar != null)
        {
            fuelBar.minValue = 0; // Ensure min value is set
            fuelBar.maxValue = fuel; // Set max value to match initial fuel
            fuelBar.value = fuel; // Start full
        }

        UpdateFuelUI();
    }

    void Update()
    {
        // Apply downward force for smooth descent
        if (rb.velocity.y > maxFallSpeed)
        {
            rb.AddForce(Vector2.down * gravityForce, ForceMode2D.Force);
        }

        // Move left/right
        float horizontalInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        // Jump (react quickly) if fuel is available
        if (Input.GetKeyDown(KeyCode.Space) && fuel > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0); // Reset velocity for snappy reaction
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            fuel -= fuelConsumption;
            UpdateFuelUI();
        }

        // Limit max upward speed
        if (rb.velocity.y > maxAscentSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, maxAscentSpeed);
        }

        // Apply swaying motion
        swayTimer += Time.deltaTime;
        float sway = Mathf.Sin(swayTimer * swaySpeed) * swayAmount;
        transform.rotation = Quaternion.Euler(0, 0, sway);
    }

    void UpdateFuelUI()
    {
        if (fuelBar != null)
        {
            fuelBar.value = Mathf.Clamp(fuel, 0, fuelBar.maxValue); // Ensure it stays within valid range
        }
    }
}
