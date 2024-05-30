using Player;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
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
        Debug.Log($"{continueButton.transform.position}  {Input.mousePosition}");
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        if (isPaused)
            Continue();
        else
            Pause();
    }

    private void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
    }

    private void Continue()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
    }

    private void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
}