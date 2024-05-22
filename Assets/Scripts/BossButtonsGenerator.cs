using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;


public class Boss_buttons_generator : MonoBehaviour
{
    private int bossHeath;
    public GameObject buttonPrefab;
    private static Dictionary<string, Texture2D> buttonTextures;
    private static string letters = "ZEQ";
    private int buttonsLimit = 10;
    private HashSet<(float, float)> buttonsCoord;
    private SpriteRenderer sprite;
    private float timeStart;
    private HashSet<GameObject> buttons;
    
    private void Awake()
    {
        timeStart = Time.deltaTime;
        buttons = new HashSet<GameObject>();
        sprite = GetComponent<SpriteRenderer>();
        buttonsCoord = new HashSet<(float, float)>();
        bossHeath = 10;
        buttonPrefab = Resources.Load<GameObject>("button");
        if (buttonPrefab == null)
            Debug.LogError("Не удалось загрузить префаб кнопки!");
        buttonTextures = Fighting.LettersTo2DTextures.ConnectCharWithTexture(letters);
    }
    
    private void Update()
    {
        if (bossHeath > 0 && buttonsCoord.Count < buttonsLimit && timeStart > 1f)
        {
            GenerateButton();
            timeStart = Time.deltaTime;
        }

        timeStart += Time.deltaTime;
        // УМЕНЬШЕНИЕ КНОПОК
        foreach (var button in buttons)
        {
            button.transform.localScale = button.transform.localScale * 0.9999f;
        }
    }
    
    public void GenerateButton()
    {
        Debug.Log(buttonsCoord.Count);
        var koef = 0.3f;
        var buttonSize = 3f;
        var button = Fighting.ButtonSequenceGen.GenerateButton(letters);
        var random = new Random();
        Vector3 buttonPosition =sprite.transform.position + new Vector3(random.Next(-5, 5), random.Next(-2, 4), 0);
        if (!IsCollidesWithButtons(buttonPosition, buttonSize))
        {
            buttonsCoord.Add(new(buttonPosition.x, buttonPosition.y));
            var texture = buttonTextures[button.ToString()];
            // Создаем экземпляр кнопки из префаба
            GameObject newButton = Instantiate(buttonPrefab, buttonPosition, Quaternion.identity, transform);
            SpriteRenderer buttonSpriteRenderer = newButton.GetComponent<SpriteRenderer>();
            newButton.transform.localScale = new Vector3(buttonSize, buttonSize, buttonSize);
            buttonSpriteRenderer.sortingOrder = 10;
            // Устанавливаем текстуру для кнопки
            buttonSpriteRenderer.sprite =
                Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            buttons.Add(newButton);
        }
    }

    private bool IsCollidesWithButtons(Vector3 buttonPosition, float buttonSize)
    {
        foreach (var button in buttonsCoord)
        {
            var diffX = Math.Abs(button.Item1 - buttonPosition.x);
            var diffY = Math.Abs(button.Item2 - buttonPosition.y);
            if (diffX <= 1.5 && diffY < 1.5)
                return true;
        }

        return false;
    }
}
