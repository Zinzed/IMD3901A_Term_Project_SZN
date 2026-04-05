using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenuUI; // Drag your Pause Panel here in the Inspector
    private bool isPaused = false;

    void Start()
    {
        // Ensure menu is hidden when the game starts
        pauseMenuUI.SetActive(false);
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Freezes game movement/physics

        // Unlock cursor so you can click the menu buttons
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resumes game movement/physics

        // Re-lock cursor 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPaused = false;
    }
}
