using System;
using UnityEngine.SceneManagement;

public static class Death
{
    private static int currentLevelIndex;
    public static int DeathScreenIndex = 0;
    
    public static void MoveToScreenDeath()
    {
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        PlayerStats.currentLevelIndex = currentLevelIndex;
        SceneManager.LoadScene(DeathScreenIndex);
    }

    public static void Respawn()
    {
        SceneManager.LoadScene(PlayerStats.currentLevelIndex);
    }
}