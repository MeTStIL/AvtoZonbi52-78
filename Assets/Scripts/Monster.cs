using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fighting;
using Player;
using Player.Health;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public interface IMonster
{
    int LivesCount { get; set; }
    float StandardSpeed { get; set; }
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
    void GenerateButtonSequence();
}

public class Monster : Sounds, IMonster
{
    public int LivesCount { get; set; }
    public float StandardSpeed { get; set; }
    [SerializeField] private float WalkingRadius;
    [SerializeField] public float VisibleRadius;
    [SerializeField] public bool IsStatic;
    public int ButtonCount { get; set; }
    public Vector3 SpritePosition { get; set; }
    public bool isButtonGenerated { get; set; }
    public Queue<char> buttonSequence { get; set; }
    
    private List<GameObject> buttonInstances = new List<GameObject>();
   
    public GameObject buttonPrefab;
    private SpriteRenderer sprite;
    public Vector3 targetPosition;
    private bool isStopped = false;
    public bool isHarassment = false;
    private Rigidbody2D rb;
    public float speedAttack;
    public float currentSpeed;
    public float freezeTime = 2f;
    [SerializeField] public Health PlayerHealth;
    public float timeForAttack;
    private bool isDead;
    private ButtonSequenceGen buttonGenerator;
    public bool? IsCorrectClick;
    
    
    public virtual void Awake()
    {
        isDead = false;
        
        buttonGenerator = new ButtonSequenceGen();
        sprite = GetComponent<SpriteRenderer>();
        buttonSequence = new Queue<char>();
        isButtonGenerated = false;
        rb = GetComponent<Rigidbody2D>();
        SpritePosition = sprite.transform.position;
        SetNewTargetPosition();
        buttonPrefab = Resources.Load<GameObject>("button");
    }

    private void CheckTimeToAttack()
    {
        isButtonGenerated = false;
        buttonInstances = new List<GameObject>();
        buttonSequence = new Queue<char>();
        while (gameObject.transform.childCount > 0)
        {
            DestroyImmediate(gameObject.transform.GetChild(0).gameObject);
        }

        timeForAttack = Time.time;
        if (objectSounds.Length > 0)
            PlaySound(objectSounds[0]);
        PlayerHealth.TakeDamage(1);
        
    }

    private void GiveDamageToMonster()
    {
        isButtonGenerated = false;
        LivesCount--;
        buttonInstances = new List<GameObject>();
        while (gameObject.transform.childCount > 0)
        {
            DestroyImmediate(gameObject.transform.GetChild(0).gameObject);
        }
    }
    
    private void CheckForAttack()
    {
        if (isButtonGenerated)
        {
            if (Time.time - timeForAttack > 2f)
                CheckTimeToAttack();
            else if (buttonSequence.Count == 0)
                GiveDamageToMonster();
        }
    }
    public virtual void LateUpdate()
    {
        
        if (LivesCount == 0)
        {
            isDead = true;
        }

        if (isStopped)
            StartCoroutine(StopAndSetNewTarget());
        Walking();
        CheckForAttack();
        
         
    }
    
    public void GenerateButtonSequence()
    {
        var buttons = buttonGenerator.GenerateButtonSeq(ButtonCount);
        Vector3 buttonPosition = sprite.transform.position + new Vector3((-ButtonCount)*0.3f, 1, 0);
        
        foreach (var letter in buttons)
        {
            var newButton = Instantiate(buttonPrefab, buttonPosition, Quaternion.identity, transform);
            var buttonInfo = new ButtonInfo(newButton, letter, sprite);
            buttonGenerator.GenerateSprite(buttonInfo, buttonSequence, buttonInstances, ButtonCount, buttonPosition);
            buttonPosition += new Vector3(1*0.8f, 0, 0);
        }
        isButtonGenerated = true;
        
    }
    
    
    #region Преследование
    public Transform player;
    public float chaseRadius;
    public void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        chaseRadius = 3f * VisibleRadius;
    }
    
    
    public void Update()
    {
        IsCorrectClick = buttonGenerator.CheckForCorrectClick(buttonSequence, buttonInstances);
        if (LivesCount == 0)
            Die();

        if (Vector3.Distance(transform.position, player.position) <= VisibleRadius 
            && isButtonGenerated == false && LivesCount > 0)
        {
            GenerateButtonSequence();
            timeForAttack = Time.time;
        }
    }
    
    #endregion
    
    #region Житуха

    public virtual void Die()
    {
        if (isDead)
            return;

        isDead = true;
        Destroy(gameObject, 0.3f);
        PlayerStats.kills += 1;
        AudioSource.PlayClipAtPoint(objectSounds[1], transform.position, 1f);
    }

    public virtual void GetDamage()
    {
        
        LivesCount -= 1;
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
        if (!collision.gameObject.CompareTag("Zombie"))
        {
            rb.isKinematic = true;
            // Останавливаем объект
            rb.velocity = Vector2.zero;
            // Вызываем метод Unfreeze через freezeTime секунд
            Invoke("Unfreeze", freezeTime);
        }
        
    }

    #endregion
    
    

}