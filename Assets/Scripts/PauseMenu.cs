using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused;
    public GameObject pauseMenuUI;
    public Button continueButton;
    public Button quitButton;

    private void Start()
    {
        pauseMenuUI.SetActive(false);
        continueButton.onClick.AddListener(Continue);
        quitButton.onClick.AddListener(LoadMainMenu);
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        if (IsPaused)
            Continue();
        else
            Pause();
    }

    private void Pause()
    {
        IsPaused = true;
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
    }

    private void Continue()
    {
        IsPaused = false;
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
    }

    private static void LoadMainMenu()
    {
        IsPaused = false;
        Time.timeScale = 1f;
        PlayerStats.Kills = 0;
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}