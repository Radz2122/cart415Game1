using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotAirBalloonController : MonoBehaviour
{
    public float gravityForce = 3.5f;
    public float moveSpeed = 3f;
    public float jumpForce = 12f;
    public float fuel = 100f;
    public float fuelConsumption = 10f;
    public float maxAscentSpeed = 2.5f;
    public float maxFallSpeed = -5f;

    public float swayAmount = 3f;
    public float swaySpeed = 2f;

    public int maxHearts = 3;
    private int currentHearts;
    public Image[] heartIcons;

    public Slider fuelBar;

    private Rigidbody2D rb;
    private float swayTimer;
    
    private bool canTakeDamage = true; // 🔹 Prevents rapid damage
    public float damageCooldown = 2f; // 🔹 Time before player can take damage again

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        currentHearts = maxHearts;
        UpdateHeartsUI();

        if (fuelBar != null)
        {
            fuelBar.minValue = 0;
            fuelBar.maxValue = fuel;
            fuelBar.value = fuel;
        }

        UpdateFuelUI();
    }

    void Update()
    {
        if (rb.velocity.y > maxFallSpeed)
        {
            rb.AddForce(Vector2.down * gravityForce, ForceMode2D.Force);
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && fuel > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            fuel -= fuelConsumption;
            UpdateFuelUI();
        }

        if (rb.velocity.y > maxAscentSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, maxAscentSpeed);
        }

        swayTimer += Time.deltaTime;
        float sway = Mathf.Sin(swayTimer * swaySpeed) * swayAmount;
        transform.rotation = Quaternion.Euler(0, 0, sway);
    }

    void UpdateFuelUI()
    {
        if (fuelBar != null)
        {
            fuelBar.value = Mathf.Clamp(fuel, 0, fuelBar.maxValue);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && canTakeDamage)
        {
            TakeDamage();
        }
    }

    void TakeDamage()
    {
        if (currentHearts > 0)
        {
            currentHearts--;
            UpdateHeartsUI();
            StartCoroutine(DamageCooldown()); // 🔹 Start cooldown timer
        }

        if (currentHearts <= 0)
        {
            GameOver();
        }
    }

    IEnumerator DamageCooldown()
    {
        canTakeDamage = false; // 🔹 Prevents further damage
        yield return new WaitForSeconds(damageCooldown); // 🔹 Wait for cooldown time
        canTakeDamage = true; // 🔹 Allows damage again
    }

    void UpdateHeartsUI()
    {
        for (int i = 0; i < heartIcons.Length; i++)
        {
            heartIcons[i].enabled = i < currentHearts;
        }
    }

    void GameOver()
    {
        Debug.Log("Game Over!");
    }
}
