using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.WSA;

public class TypingText : MonoBehaviour
{
    public float delay = 0.1f;
    private string messagesPath;
    private List<string> messages;
    private string imagesPath;
    private Dictionary<int, Texture2D> images;
    [SerializeField] private Image image;
    private Text textComp;
    private bool isActive;
    private int currentIndex;
    void Start()
    {
        imagesPath = "Assets/StartCutScene/Images";
        messagesPath = "Assets/StartCutScene/texts.txt";
        messages = ImageMessageConvertion.GetTexts(messagesPath);
        images = ImageMessageConvertion.GetImages(imagesPath);
        isActive = false;
        
        textComp = GetComponent<Text>();
        textComp.text = "";
        Debug.Log(messages.Count);
        Debug.Log(images.Count);
        Debug.Log(images[0]);
        currentIndex = 0;
    }

    IEnumerator ShowText(int index)
    {
        textComp.text = "";
        image.sprite = Sprite.Create(images[index], new Rect(0, 0, images[index].width, images[index].height), Vector2.zero);
        image.color = Color.white;
        isActive = true;
        foreach (char c in messages[index])
        {
            textComp.text += c;
            yield return new WaitForSeconds(delay);
        }

        StartCoroutine(Delay(2));
        
        
    }

    private void Update()
    {
        if (currentIndex == 4)
        {
            StartCoroutine(Delay(2));
            SceneManager.LoadScene(1);
        }
        if (!isActive && currentIndex < 4)
            StartCoroutine(ShowText(currentIndex));
        
    }
    
    IEnumerator Delay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        currentIndex += 1;
        isActive = false;
    }
}
