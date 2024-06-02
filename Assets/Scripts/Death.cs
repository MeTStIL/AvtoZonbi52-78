using System;
using UnityEngine.SceneManagement;

public static class Death
{
    private static int currentLevelIndex;
    public static int DeathScreenIndex = 1;
    
    
    public static void MoveToScreenDeath()
    {
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        PlayerStats.CurrentLevelIndex = currentLevelIndex;
        SceneManager.LoadScene(DeathScreenIndex);
    }

    public static void Respawn()
    {
        PlayerStats.Kills = 0;
        SceneManager.LoadScene(PlayerStats.CurrentLevelIndex, LoadSceneMode.Single);
    }
}