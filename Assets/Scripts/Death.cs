using UnityEngine.SceneManagement;

public static class Death
{
    private static int currentLevelIndex;
    private const int DeathScreenIndex = 1;

    public static void MoveToScreenDeath()
    {
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        PlayerStats.currentLevelIndex = currentLevelIndex;
        SceneManager.LoadScene(DeathScreenIndex);
    }

    public static void Respawn()
    {
        PlayerStats.kills = 0;
        SceneManager.LoadScene(PlayerStats.currentLevelIndex, LoadSceneMode.Single);
    }
}