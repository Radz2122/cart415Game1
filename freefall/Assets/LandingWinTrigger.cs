using UnityEngine;
using UnityEngine.SceneManagement; // Needed for reloading scenes
using UnityEngine.UI; // Needed for UI elements

public class LandingWinTrigger : MonoBehaviour
{
    public GameObject winScreenUI; // Assign in Inspector

    void Start()
    {
        if (winScreenUI != null)
            winScreenUI.SetActive(false); // Hide win screen at start
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("landed")) // Check if balloon touches platform
        {
            ShowWinScreen();
        }
    }

    void ShowWinScreen()
    {
        if (winScreenUI != null)
        {
            winScreenUI.SetActive(true);
            Time.timeScale = 0; // Pause the game when landing
        }
    }

    // 🔹 Restart Game Function (Call this from UI Button)
   public void RestartGame()
{
     Debug.Log("Restart button clicked!");
    Time.timeScale = 1; // ✅ Unpause game before restarting
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // ✅ Reload current scene
}

}
