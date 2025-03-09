using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

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

    private bool canTakeDamage = true; // Prevents rapid damage
    public float damageCooldown = 2f; // Time before player can take damage again

    public Button restartButton; // Assign the Game Over panel in the Inspector

    // Tornado Effect
    public float tornadoSpeedBoost = 3f; //  Additional downward speed inside tornado
    public float tornadoEffectDuration = 1f; //  Duration inside tornado
    private bool inTornado = false;
    public GameObject loseScreenUI; // Assign in Inspector

    public AudioSource collisionSound;
    public AudioSource jumpSound;
    public AudioSource tornadoSound;

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

        if (restartButton != null)
            restartButton.gameObject.SetActive(false); //  Enables the Button's GameObject


        if (loseScreenUI != null)
            loseScreenUI.SetActive(false); // Hide win screen at start

          Time.timeScale = 1; //  Ensure game starts unpaused

           //  Ensure AudioSources are assigned
    if (collisionSound == null || jumpSound == null || tornadoSound == null)
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        if (audioSources.Length >= 3)
        {
            collisionSound = audioSources[0];
            jumpSound = audioSources[1];
            tornadoSound = audioSources[2];
        }
    }
    }

    void Update()
    {
        // Apply normal downward force
        if (rb.velocity.y > maxFallSpeed && !inTornado)
        {
            rb.AddForce(Vector2.down * gravityForce, ForceMode2D.Force);
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && fuel > 0)
        {
            if (jumpSound != null)
        {
            jumpSound.Play(); //  Play jump sound
        }
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
              if (collisionSound != null)
        {
            collisionSound.Play(); //  Play collision sound
        }
        
         CameraShake cameraShake = Camera.main.GetComponent<CameraShake>();
        if (cameraShake != null)
        {
            StartCoroutine(cameraShake.Shake());
        }
        else
        {
            Debug.LogError(" CameraShake script is missing on Main Camera!");
        }

            TakeDamage();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Tornado")) // Ensure the tornado has this tag
        {
            if (tornadoSound != null)
            {
                tornadoSound.Play(); //  Play tornado sound
            }
            StartCoroutine(ApplyTornadoEffect());
        }
    }

   IEnumerator ApplyTornadoEffect()
{
    inTornado = true;
    float originalMoveSpeed = moveSpeed;

    //  Increase move speed (horizontal movement)
    moveSpeed += tornadoSpeedBoost;
    
    //  Instantly increase downward velocity (forces faster fall)
    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - tornadoSpeedBoost);

    yield return new WaitForSeconds(tornadoEffectDuration); // Wait for effect duration

    //  Immediately restore original move speed & reset velocity
    moveSpeed = originalMoveSpeed;
    rb.velocity = new Vector2(rb.velocity.x, -gravityForce); //  Reset downward velocity

    inTornado = false;
}


    void TakeDamage()
    {
        if (currentHearts > 0)
        {
            currentHearts--;
            UpdateHeartsUI();
            StartCoroutine(DamageCooldown());
        }

        if (currentHearts <= 0)
        {
            GameOver();
        }
    }

    IEnumerator DamageCooldown()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
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

    //  Stop camera shake when game ends
    CameraShake cameraShake = Camera.main.GetComponent<CameraShake>();
    if (cameraShake != null)
    {
        StopAllCoroutines(); //  Stop all ongoing shakes
        cameraShake.StopAllCoroutines(); //  Ensure shake stops
        cameraShake.transform.position = cameraShake.transform.position; //  Reset camera position
    }

    if (restartButton != null)
    {
        restartButton.gameObject.SetActive(true);
        restartButton.interactable = true;
        restartButton.onClick.RemoveAllListeners();
        restartButton.onClick.AddListener(RestartGame);
    }

    if (loseScreenUI != null)
    {
        loseScreenUI.SetActive(true);
    }

    Time.timeScale = 0; //  Pause the game
}

 public void RestartGame()
    {
        Debug.Log("Restart button clicked! Restarting game...");
        Time.timeScale = 1; //  Ensure the game is unpaused before restarting
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
