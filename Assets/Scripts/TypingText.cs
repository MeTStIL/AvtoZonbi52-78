using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.WSA;

public class TypingText : Sounds
{
    private float startTime;
    [SerializeField] private GameObject skipButton;
    [SerializeField] private GameObject skipText;
    public float delay = 0.1f;
    private readonly float visabilityCoef = 0.1f;
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
        startTime = Time.deltaTime;
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
        SetImage(index);
        isActive = true;
        for (var i = 0; i < messages[index].Length; i++)
        {
            var c = messages[index][i];
            IncreaseImageVisibility();
            textComp.text += c;
            if (i % 2 == 0)
                PlaySound(objectSounds[0]);
            yield return new WaitForSeconds(delay);
        }

        StartCoroutine(Delay(2));
        
    }

    private void SetImage(int index)
    {
        image.sprite = Sprite.Create(images[index], new Rect(0, 0, images[index].width, images[index].height), Vector2.zero);
        image.color = Color.white;
        var color = image.color;
        color.a = 0;
        image.color = color;
    }

    private void IncreaseImageVisibility()
    {
        var color = image.color;
        color.a += visabilityCoef;
        image.color = color;
    }

    private void Update()
    {
        EnableExitButton();
        MoveToNextScene();
        if (!isActive && currentIndex < 4)
            StartCoroutine(ShowText(currentIndex));
        if (Input.GetKeyDown(KeyCode.Q))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    
    IEnumerator Delay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        currentIndex += 1;
        isActive = false;
    }
    IEnumerator StartDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        isActive = false;
    }

    private void MoveToNextScene()
    {
        
        if (currentIndex != 4) return;
        StartCoroutine(Delay(2));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    private void EnableExitButton()
    {
        startTime += Time.deltaTime;
        if (!(Math.Abs(startTime - 3f) < 0.3f)) return;
        skipButton.SetActive(true);
        skipText.SetActive(true);
    }
}
