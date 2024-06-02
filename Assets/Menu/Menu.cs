using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void ChangeScene()
    {
        SceneManager.LoadScene(2); 
    }

    public void Exit()
    {
        Application.Quit();
    }
}
