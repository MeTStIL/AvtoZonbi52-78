using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TypingText : Sounds
{
    private float startTime;
    [SerializeField] private GameObject skipButton;
    [SerializeField] private int nextSceneIndex;
    [SerializeField] private GameObject skipText;
    public float delay = 0.05f;
    private const float VisibilityCoefficient = 0.1f;
    [SerializeField]private string messagesPath;
    private List<string> messages;
    [SerializeField]private string imagesPath;
    private Dictionary<int, Texture2D> images;
    [SerializeField] private Image image;
    private Text textComp;
    private bool isActive;
    private int currentIndex;

    private void Start()
    {
        messages = ImageMessageConvertion.GetTexts(messagesPath);
        images = ImageMessageConvertion.GetImages(imagesPath);
        isActive = false;
        startTime = Time.deltaTime;
        textComp = GetComponent<Text>();
        textComp.text = "";
        currentIndex = 0;
    }

    private IEnumerator ShowText(int index)
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
        color.a += VisibilityCoefficient;
        image.color = color;
    }

    private void Update()
    {
        EnableExitButton();
        MoveToNextScene();
        if (!isActive && currentIndex < messages.Count)
            StartCoroutine(ShowText(currentIndex));
        if (Input.GetKeyDown(KeyCode.Q))
            SceneManager.LoadScene(nextSceneIndex);
    }

    private IEnumerator Delay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        currentIndex += 1;
        isActive = false;
    }

    private void MoveToNextScene()
    {
        
        if (currentIndex != messages.Count) return;
        StartCoroutine(Delay(2));
        SceneManager.LoadScene(nextSceneIndex);
    }
    private void EnableExitButton()
    {
        startTime += Time.deltaTime;
        if (!(Math.Abs(startTime - 3f) < 0.3f)) return;
        skipButton.SetActive(true);
        skipText.SetActive(true);
    }
}
