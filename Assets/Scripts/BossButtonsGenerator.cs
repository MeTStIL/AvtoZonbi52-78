using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;
using UnityEngine.SceneManagement;

public class Boss_buttons_generator : Sounds
{
    [SerializeField] private RawImage bossHealthBar;
    [SerializeField] private RawImage playerHealthBar;
    [SerializeField] private string BossType;
    private int bossHeath;
    private int playerHealth;
    public GameObject buttonPrefab;
    private static Dictionary<string, Texture2D> buttonTextures;
    private static string letters = "ZEQ";
    private int buttonsLimit = 10;
    private HashSet<(float, float)> buttonsCoord;
    private SpriteRenderer sprite;
    private bool isSecondWave;
    private bool isLastWave;
    private float timeStart;
    private Dictionary<GameObject, KeyCode> buttons;
    private HashSet<GameObject> deletedButtons;
    private int applyButtonsCount;
    private Vector3 buttonDecreaseKoef;
    private float buttonVisibilityKoef;
    private float buttonsGenTimeDelay;
    private bool isDelay;
    private int buttonsToDamage;
    private bool isPaused = false;
        
    private void Awake()
    {
        buttonsToDamage = 10;
        isDelay = false;
        buttonsGenTimeDelay = 0.5f;
        isSecondWave = false;
        isLastWave = false;
        buttonDecreaseKoef = new Vector3(0.003f, 0.003f, 0.003f);
        buttonVisibilityKoef = 0.0005f;
        applyButtonsCount = 0;   
        timeStart = Time.deltaTime;
        deletedButtons = new HashSet<GameObject>();
        buttons = new Dictionary<GameObject, KeyCode>();
        sprite = GetComponent<SpriteRenderer>();
        buttonsCoord = new HashSet<(float, float)>();
        bossHeath = 8;
        playerHealth = 8;
        buttonPrefab = Resources.Load<GameObject>("button");
        if (buttonPrefab == null)
            Debug.LogError("Не удалось загрузить префаб кнопки!");
        buttonTextures = Fighting.LettersTo2DTextures.ConnectCharWithTexture(letters);
    }
    
    private void Update()
    {
        var isButtonCorrect = false;
        if (applyButtonsCount == buttonsToDamage)
        {
            DamageBoss();
            applyButtonsCount = 0;
            
        }

        if (bossHeath > 0 && buttons.Count-deletedButtons.Count < buttonsLimit && timeStart > buttonsGenTimeDelay)
        {
            isDelay = false;
            GenerateButton();
            timeStart = Time.deltaTime;
        }
        
        foreach (var genButton in buttons)
        {
            if (!deletedButtons.Contains(genButton.Key))
            {
                if (genButton.Key.transform.localScale.x < 2f)
                {
                    deletedButtons.Add(genButton.Key);
                    ChangeButtonColor(genButton.Key, genButton.Value, "cancel");
                    Destroy(genButton.Key, 0.3f);
                    DamagePlayer();

                }
            }
            if (Input.GetKeyDown(genButton.Value) && !deletedButtons.Contains(genButton.Key)) 
            {
                Debug.Log("ПРАВИЛЬНО НАЖАЛ");
                
                isButtonCorrect = true;
                deletedButtons.Add(genButton.Key);
                var position = genButton.Key.transform.position;
                Debug.Log(position);

                ChangeButtonColor(genButton.Key, genButton.Value, "apply");
                PlaySound(objectSounds[0], volume: 0.8f, fadeInTime: 0f);
                applyButtonsCount += 1;
                Destroy(genButton.Key, 0.3f);
                break;
            }
        }
        if (!isButtonCorrect && Input.anyKeyDown && !isDelay)
            DamagePlayer();

        timeStart += Time.deltaTime;
        DecreasingButtons();
    }

    private void DamageBoss()
    {
        
        bossHeath -= 1;
        Debug.Log("ЖИЗНЬ БОССА " + bossHeath);
        var texture = Fighting.LettersTo2DTextures.LoadTextureFromPath($"Assets/{BossType}HealthBar/{bossHeath}.png");
        bossHealthBar.texture = texture;
        if (bossHeath == 4 && isSecondWave == false)
            MakeSecondWave();
        if (bossHeath == 1 && isLastWave == false)
            MakeLastWave();
        if (bossHeath == 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
    }

    private void MakeWaveDelay()
    {
        isDelay = true;
        foreach (var genButton in buttons)
        {
            if (!deletedButtons.Contains(genButton.Key))
            {
                deletedButtons.Add(genButton.Key);
                Destroy(genButton.Key, 0.3f);

            }
        }

        timeStart = -3f;
    }
    
    private void DamagePlayer()
    {
        PlaySound(objectSounds[3]);
        playerHealth -= 1;
        Debug.Log("ЖИЗНЬ БОССА " + playerHealth);
        var texture = Fighting.LettersTo2DTextures.LoadTextureFromPath($"Assets/PlayerHealthBar/{playerHealth}.png");
        playerHealthBar.texture = texture;
        if (playerHealth == 0)
            Death.MoveToScreenDeath();
        
    }

    private void MakeSecondWave()
    {
        PlaySound(objectSounds[1]);
        buttonDecreaseKoef = new Vector3(0.005f, 0.005f, 0.005f);
        isSecondWave = true;
        MakeWaveDelay();
    }
    
    private void MakeLastWave()
    {
        PlaySound(objectSounds[2]);
        buttonDecreaseKoef = new Vector3(0.006f, 0.006f, 0.006f);
        isLastWave = true;
        buttonsToDamage = 20;
        buttonsGenTimeDelay = 0.3f;
        MakeWaveDelay();
        
    }
    
    private void ChangeButtonColor(GameObject button, KeyCode letter, string type)
    {
        var texture = buttonTextures[letter + type];
        Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),Vector2.zero);
        button.GetComponent<SpriteRenderer>()
            .sprite = newSprite;
    }
    private void DecreasingButtons()
    {
        foreach (var button in buttons.Keys.Where((k, v) => !deletedButtons.Contains(k)))
        {
            isPaused = PauseMenu.isPaused;
            if (!isPaused)
            {
                button.transform.localScale -= buttonDecreaseKoef;
                var buttonSprite = button.GetComponent<SpriteRenderer>();
                var color = buttonSprite.color;
                color.a -= buttonVisibilityKoef;
                buttonSprite.color = color;
            }
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
