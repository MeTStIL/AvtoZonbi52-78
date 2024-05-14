using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToOtherScene : MonoBehaviour
{
    

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("ПОДОШЁЛ");
        if (other.gameObject.CompareTag("Player"))
        {
            var index = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(index);
        }
    }
    
}
