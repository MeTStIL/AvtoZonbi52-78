using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;
using Random = UnityEngine.Random;

public interface IMonster
{
    int LivesCount { get; set; }
    float StandardSpeed { get; set; }
    float WalkingRadius { get; set; }
    float VisibleRadius { get; set; }
    Vector3 SpritePosition { get; set; }
    public int ButtonCount { get; set; }
    bool isButtonGenerated { get; set; }
    Queue<char> buttonSequence { get; set; }

    void Awake();
    void LateUpdate();
    void Die();
    void GetDamage();
    void Walking();
    IEnumerator StopAndSetNewTarget();
    void SetNewTargetPosition();
    void StartHarassment();
    void GenerateButtonSequence();
}

public class Monster : MonoBehaviour, IMonster
{
    
    public int LivesCount { get; set; }
    public float StandardSpeed { get; set; }
    public float WalkingRadius { get; set; }
    public float VisibleRadius { get; set; }
    public int ButtonCount { get; set; }
    public Vector3 SpritePosition { get; set; }
    public bool isButtonGenerated { get; set; }
    public Queue<char> buttonSequence { get; set; }

    private static string letters = "AEQ";
    private static Dictionary<string, Texture2D> buttonTextures;
    private List<GameObject> buttonInstances = new List<GameObject>();
    private Collider2D collider;
    public GameObject buttonPrefab;
    private SpriteRenderer sprite;
    private Vector3 targetPosition;
    private bool isStopped = false;
    public bool isHarassment = false;
    public LayerMask obstacleLayer;
    private Rigidbody2D rb;
    public float speedAttack;
    public float currentSpeed;
    public float freezeTime = 2f;

    public virtual void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        buttonSequence = new Queue<char>();
        isButtonGenerated = false;
        collider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        SpritePosition = sprite.transform.position;
        SetNewTargetPosition();
        buttonPrefab = Resources.Load<GameObject>("button");
        if (buttonPrefab == null)
        {
            Debug.LogError("Не удалось загрузить префаб кнопки!");
        }
        buttonTextures = Fighting.LettersTo2DTextures.ConnectCharWithTexture(letters);
    }

    public virtual void LateUpdate()
    {
        if (isStopped)
            StartCoroutine(StopAndSetNewTarget());
        Walking();
        
        // Нажатие кнопок
        if (isButtonGenerated == true && buttonSequence.Count > 0)
        {
            var currentButton = Fighting.ButtonSequenceGen.buttons[buttonSequence.First()];
            if (Input.GetKeyDown(currentButton))
            {
                Debug.Log("Нажали");
                buttonSequence.Dequeue();
                if (buttonTextures.ContainsKey(currentButton.ToString() + "apply"))
                {
                    Debug.Log("Загрузили текстурку");
                    var texture = buttonTextures[currentButton + "apply"];
                    var currentSprite = buttonInstances[buttonInstances.Count - buttonSequence.Count - 1];
                    var position = currentSprite.transform.position;
                    Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),Vector2.zero);
                    
                    buttonInstances[buttonInstances.Count - buttonSequence.Count - 1].GetComponent<SpriteRenderer>()
                        .sprite = newSprite;
                }
                else
                {
                    Debug.Log("с текстурами прроблема");
                }
            }
            else if (Input.anyKeyDown)
            {
                Debug.Log("Загрузили текстурку");
                var texture = buttonTextures[currentButton + "cancel"];
                var currentSprite = buttonInstances[buttonInstances.Count - buttonSequence.Count];
                var position = currentSprite.transform.position;
                Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),Vector2.zero);
                    
                buttonInstances[buttonInstances.Count - buttonSequence.Count].GetComponent<SpriteRenderer>()
                    .sprite = newSprite;
            }
        }
        
            
        
    }

    #region Buttons

    public void GenerateButtonSequence()
    {
        var koef = 0.3f;
        var buttons = Fighting.ButtonSequenceGen.GenerateButtonSeq(letters, ButtonCount);
        Vector3 buttonPosition = sprite.transform.position + new Vector3((-ButtonCount)*koef, 1, 0);
        
        foreach (var letter in buttons)
        {
            buttonSequence.Enqueue(letter);
            var texture = buttonTextures[letter.ToString()];
            // Создаем экземпляр кнопки из префаба
            GameObject newButton = Instantiate(buttonPrefab, buttonPosition, Quaternion.identity, transform);
            SpriteRenderer buttonSpriteRenderer = newButton.GetComponent<SpriteRenderer>();
            newButton.transform.localScale = new Vector3(2, 2, 2);
            // Устанавливаем текстуру для кнопки
            buttonSpriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            // Смещаем позицию для следующей кнопки
            buttonPosition += new Vector3(1*0.8f, 0, 0); // Примерное смещение, в зависимости от вашего дизайна
            // Добавляем кнопку в список
            buttonInstances.Add(newButton);
        }

        isButtonGenerated = true;
        
    }

     
    #endregion
    
    #region Преследование
    private Transform player;
    private float chaseRadius;
    public void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        chaseRadius = 3f * VisibleRadius;
    }
    
    
    public void Update()
    {
        
        if (Vector3.Distance(transform.position, player.position) <= VisibleRadius 
            && isButtonGenerated == false)
        {
            GenerateButtonSequence();
        }
        /*if (Vector3.Distance(transform.position, player.position) <= VisibleRadius)
        {
            if (Vector3.Distance(transform.position, player.position) <= 5000)
                StartCoroutine(StopTest());
            isHarassment = true;
            currentSpeed = speedAttack;
            StartHarassment();
        }
        else if (Vector3.Distance(transform.position, player.position) > chaseRadius)
        {
            isHarassment = false;
            currentSpeed = StandardSpeed;
            StopHarassment();
        }*/
    }

    public void StartHarassment()
    {
        targetPosition = player.position;
    }

    public void StopHarassment()
    {
        SetNewTargetPosition();
    }
    
    #endregion
    
    #region Житуха

    public virtual void Die()
    {
        Destroy(gameObject, 1.4f);
    }

    public virtual void GetDamage()
    {
        LivesCount -= 1;
        if (LivesCount == 0)
            Die();
    }

    #endregion

    #region Движение

    public virtual void Walking()
    {
        if (Vector3.Distance(sprite.transform.position, targetPosition) > 0.1f && rb.isKinematic == false)
        {
            Vector3 direction = (targetPosition - sprite.transform.position).normalized;
            sprite.transform.position = Vector3.MoveTowards(sprite.transform.position, targetPosition, currentSpeed * Time.deltaTime);
        }
        else if (isHarassment == false)
        {
            StartCoroutine(StopAndSetNewTarget());
        }
    }

    public IEnumerator StopAndSetNewTarget()
    {
        isStopped = true;
        yield return new WaitForSeconds(2f); // Остановка на 2 секунды
        SetNewTargetPosition();
        isStopped = false;
    }
    
    public IEnumerator StopTest()
    {
        yield return new WaitForSeconds(4f); // Остановка на 2 секунды
    }

    public void SetNewTargetPosition()
    {
        var newX = Random.Range(-WalkingRadius + SpritePosition.x, WalkingRadius + SpritePosition.x);
        var newY = Random.Range(-WalkingRadius + SpritePosition.y, WalkingRadius + SpritePosition.y);
        targetPosition = new Vector3(newX, newY, sprite.transform.position.z);
    }


    #endregion
    
    #region Заморозка при столкновениях

    private void Unfreeze()
    {
        // Размораживаем объект
        rb.isKinematic = false;
        Vector3 direction = -(targetPosition - sprite.transform.position).normalized;
        sprite.transform.position = Vector3.MoveTowards(sprite.transform.position, targetPosition, currentSpeed * Time.deltaTime);
        SetNewTargetPosition();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb.isKinematic = true;
        // Останавливаем объект
        rb.velocity = Vector2.zero;
        // Вызываем метод Unfreeze через freezeTime секунд
        Invoke("Unfreeze", freezeTime);
    }

    #endregion
    

    
}