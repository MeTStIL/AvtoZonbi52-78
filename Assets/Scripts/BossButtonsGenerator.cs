using System;
using System.Collections.Generic;
using System.Linq;
using Fighting;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;
using UnityEngine.SceneManagement;

public class BossButtonsGenerator : Sounds
{
    [SerializeField] private RawImage bossHealthBar;
    [SerializeField] private RawImage playerHealthBar;
    [SerializeField] private string BossType;
    private int bossHeath;
    private int playerHealth;
    public GameObject buttonPrefab;
    private static Dictionary<string, Texture2D> buttonTextures;
    private static string letters = "EQZCFR";
    private readonly int buttonsLimit = 10;
    private SpriteRenderer sprite;
    private bool isSecondWave;
    private bool isLastWave;
    private float timeStart;
    private Dictionary<GameObject, KeyCode> buttons;
    private HashSet<GameObject> deletedButtons;
    private int applyButtonsCount;
    private Vector3 buttonDecreaseParameter;
    private float buttonVisibilityParameter;
    private float buttonsGenTimeDelay;
    private bool isDelay;
    private int buttonsToDamage;
    private bool isPaused;
    private ButtonSequenceGen buttonGenerator;
    private bool isButtonCorrect;

    private void Awake()
    {
        buttonGenerator = new ButtonSequenceGen();
        buttonsToDamage = 10;
        buttonsGenTimeDelay = 0.5f;
        buttonDecreaseParameter = new Vector3(0.003f, 0.003f, 0.003f);
        buttonVisibilityParameter = 0.0005f;
        applyButtonsCount = 0;   
        timeStart = Time.deltaTime;
        deletedButtons = new HashSet<GameObject>();
        buttons = new Dictionary<GameObject, KeyCode>();
        sprite = GetComponent<SpriteRenderer>();
        bossHeath = 8;
        playerHealth = 8;
        buttonPrefab = Resources.Load<GameObject>("button");
        buttonTextures = LettersTo2DTextures.ConnectCharWithTexture(letters);
    }

    private void TryGenerateButton()
    {
        if (bossHeath <= 0 || buttons.Count - deletedButtons.Count >= buttonsLimit ||
            !(timeStart > buttonsGenTimeDelay)) return;
        isDelay = false;
        GenerateButton();
        timeStart = Time.deltaTime;
    }

    private bool CheckForIncorrectClick()
    {
        if (!isButtonCorrect && Input.anyKeyDown && !isDelay)
        {
            DamagePlayer();
            return true;
        }

        return false;
    }

    private bool CheckForCorrectClick(KeyValuePair<GameObject, KeyCode> genButton)
    {
        if (!Input.GetKeyDown(genButton.Value) || deletedButtons.Contains(genButton.Key)) return false;
        isButtonCorrect = true;
        deletedButtons.Add(genButton.Key);
        ChangeButtonColor(genButton.Key, genButton.Value, "apply");
        PlaySound(objectSounds[0], volume: 0.8f, fadeInTime: 0f);
        applyButtonsCount += 1;
        Destroy(genButton.Key, 0.3f);
        return true;
    }

    private void CheckForUnClicked(KeyValuePair<GameObject, KeyCode> genButton)
    {
        if (!(genButton.Key.transform.localScale.x < 2f)) return;
        deletedButtons.Add(genButton.Key);
        ChangeButtonColor(genButton.Key, genButton.Value, "cancel");
        Destroy(genButton.Key, 0.3f);
        DamagePlayer();
    }

    private void CheckPlayerInput()
    {
        foreach (var genButton in buttons)
        {
            if (!deletedButtons.Contains(genButton.Key))
            {
                CheckForUnClicked(genButton);
                if (CheckForCorrectClick(genButton))
                    break;
                if (CheckForIncorrectClick())
                    break;
            }
        }
        
    }
    
    private void Update()
    {
        if (playerHealth == 0)
            Death.MoveToScreenDeath();
        isButtonCorrect = false;
        CheckForBossDamage();
        TryGenerateButton();
        CheckPlayerInput();
        DecreasingButtons();
        timeStart += Time.deltaTime;
        
    }

    private void CheckForBossDamage()
    {
        if (applyButtonsCount != buttonsToDamage) return;
        DamageBoss();
        applyButtonsCount = 0;
    }
    
    

    private void MakeWaveDelay()
    {
        isDelay = true;
        foreach (var genButton in buttons)
        {
            if (deletedButtons.Contains(genButton.Key)) continue;
            deletedButtons.Add(genButton.Key);
            Destroy(genButton.Key, 0.3f);
        }
        timeStart = -3f;
    }
    
    private void DamageBoss()
    {
        bossHeath -= 1;
        var texture = LettersTo2DTextures.LoadTextureFromPath($"Assets/{BossType}HealthBar/{bossHeath}.png");
        bossHealthBar.texture = texture;
        if (bossHeath == 4 && isSecondWave == false)
            MakeSecondWave();
        if (bossHeath == 1 && isLastWave == false)
            MakeLastWave();
        if (bossHeath == 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
    }
    
    private void DamagePlayer()
    {
        if (playerHealth == 0)
        {
            Death.MoveToScreenDeath();
            return;
        }

        PlaySound(objectSounds[3]);
        playerHealth -= 1;
        var texture = LettersTo2DTextures.LoadTextureFromPath($"Assets/PlayerHealthBar/{playerHealth}.png");
        playerHealthBar.texture = texture;
        
    }

    private void MakeSecondWave()
    {
        PlaySound(objectSounds[1]);
        buttonDecreaseParameter = new Vector3(0.005f, 0.005f, 0.005f);
        isSecondWave = true;
        MakeWaveDelay();
    }
    
    private void MakeLastWave()
    {
        PlaySound(objectSounds[2]);
        buttonDecreaseParameter = new Vector3(0.006f, 0.006f, 0.006f);
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
        foreach (var button in buttons.Keys.Where((k, _) => !deletedButtons.Contains(k)))
        {
            isPaused = PauseMenu.isPaused;
            if (isPaused) continue;
            button.transform.localScale -= buttonDecreaseParameter;
            var buttonSprite = button.GetComponent<SpriteRenderer>();
            var color = buttonSprite.color;
            color.a -= buttonVisibilityParameter;
            buttonSprite.color = color;
        }
    }

    private void GenerateButton()
    {
        var buttonSize = 5f;
        var button = buttonGenerator.GenerateButton();
        var random = new Random();
        var buttonPosition =sprite.transform.position + new Vector3(random.Next(-5, 5), 
            random.Next(-2, 4), 0);
        if (IsCollidesWithButtons(buttonPosition)) return;
        var texture = buttonTextures[button.ToString()];
        var newButton = Instantiate(buttonPrefab, buttonPosition, Quaternion.identity, transform);
        var buttonSpriteRenderer = newButton.GetComponent<SpriteRenderer>();
        newButton.transform.localScale = new Vector3(buttonSize, buttonSize, buttonSize);
        buttonSpriteRenderer.sortingOrder = 10;
        buttonSpriteRenderer.sprite =
            Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        buttons[newButton] = ButtonsGenerationInfo.p_Buttons[button];
    }

    private bool IsCollidesWithButtons(Vector3 buttonPosition)
    {
        return (from button in buttons.Keys
            .Where((k, _) => !deletedButtons
                .Contains(k))
            let x = button.transform.position.x 
            let y = button.transform.position.y 
            let diffX = Math.Abs(x - buttonPosition.x) 
            let diffY = Math.Abs(y - buttonPosition.y) 
            where diffX <= 1.5 && diffY < 1.5 
            select diffX).Any();
    }
}
