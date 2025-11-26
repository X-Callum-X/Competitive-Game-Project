using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;

    public bool isPaused;

    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !isPaused)
        {
            OnPause();
        }
    }

    public void OnResume()
    {

        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            isPaused = false;
            pauseMenu.SetActive(false);
            Time.timeScale = 1.0f;
        }
    }

    public void OnPause()
    {
        if (!isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            isPaused = true;
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
