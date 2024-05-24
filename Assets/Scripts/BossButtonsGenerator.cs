using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private Dictionary<GameObject, KeyCode> buttons;
    private HashSet<GameObject> deletedButtons;
    
    private void Awake()
    {
        timeStart = Time.deltaTime;
        deletedButtons = new HashSet<GameObject>();
        buttons = new Dictionary<GameObject, KeyCode>();
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
        if (bossHeath > 0 && buttons.Count-deletedButtons.Count < buttonsLimit && timeStart > 0.5f)
        {
            GenerateButton();
            timeStart = Time.deltaTime;
        }

        foreach (var genButton in buttons)
        {
            if (!deletedButtons.Contains(genButton.Key))
            {
                if (genButton.Key.transform.localScale.x < 3.3f)
                {
                    deletedButtons.Add(genButton.Key);
                    var texture = buttonTextures[genButton.Value + "cancel"];
                    var currentSprite = genButton.Key;
                    Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),Vector2.zero);
                    
                    genButton.Key.GetComponent<SpriteRenderer>()
                        .sprite = newSprite;
                    //
                    Destroy(genButton.Key, 0.3f);
                    
                }
            }
            if (Input.GetKeyDown(genButton.Value) && !deletedButtons.Contains(genButton.Key)) 
            {
                Debug.Log("ПРАВИЛЬНО НАЖАЛ");
                deletedButtons.Add(genButton.Key);
                var position = genButton.Key.transform.position;
                Debug.Log(position);
                
                //ЦВЕТ
                var texture = buttonTextures[genButton.Value + "apply"];
                var currentSprite = genButton.Key;
                Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),Vector2.zero);
                    
                genButton.Key.GetComponent<SpriteRenderer>()
                    .sprite = newSprite;
                //
                Destroy(genButton.Key, 0.3f);
                break;

            }
        }

        timeStart += Time.deltaTime;
        // УМЕНЬШЕНИЕ КНОПОК
        foreach (var button in buttons.Keys.Where((k, v) => !deletedButtons.Contains(k)))
        {
            button.transform.localScale = button.transform.localScale * 0.995f;
            var buttonSprite = button.GetComponent<SpriteRenderer>();
            var color = buttonSprite.color;
            color.a -= 0.005f;
            buttonSprite.color = color;
        }
    }
    
    public void GenerateButton()
    {
        Debug.Log(buttons.Count - deletedButtons.Count);
        var koef = 0.3f;
        var buttonSize = 5f;
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
            buttons[newButton] = Fighting.ButtonSequenceGen.buttons[button];
        }
    }

    private bool IsCollidesWithButtons(Vector3 buttonPosition, float buttonSize)
    {
        foreach (var button in buttons.Keys.Where((k, v) => !deletedButtons.Contains(k)))
        {
            var x = button.transform.position.x;
            var y = button.transform.position.y;
            var diffX = Math.Abs(x - buttonPosition.x);
            var diffY = Math.Abs(y - buttonPosition.y);
            if (diffX <= 1.5 && diffY < 1.5)
                return true;
        }

        return false;
    }
}
