using System.Collections;
using Player.Health;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Monster : Sounds, IMonster
{
    [FormerlySerializedAs("WalkingRadius")] [SerializeField] private float walkingRadius;
    [FormerlySerializedAs("VisibleRadius")] [SerializeField] public float visibleRadius;
    [FormerlySerializedAs("IsStatic")] [SerializeField] public bool isStatic;
    [FormerlySerializedAs("PlayerHealth")] [SerializeField] public Health playerHealth;
    public int ButtonCount { get; set; }
    public Vector3 SpritePosition { get; set; }
    public MonsterInfo MonsterInfo { get; set; }
    public GameObject buttonPrefab;
    public SpriteRenderer sprite;
    public Vector3 targetPosition;
    private bool isStopped;
    public bool isHarassment;
    private Rigidbody2D rb;
    public float freezeTime = 0.3f;
    private bool isDead;
    public bool? IsCorrectClick;
    public Transform player;
    
    public void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }
    
    public virtual void Awake()
    {
        isDead = false;
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        SpritePosition = sprite.transform.position;
        SetNewTargetPosition();
        buttonPrefab = Resources.Load<GameObject>("button");
    }
    
    public void LateUpdate()
    {
        if (isStopped)
            StartCoroutine(StopAndSetNewTarget());
        Walking();
    }
    
    public void Update()
    {
        if (MonsterInfo.LivesCount == 0)
            Die();
    }
    
    public void Die()
    {
        if (isDead) 
            return;
        isDead = true;
        Destroy(gameObject, 0.3f);
        PlayerStats.kills += 1;
        AudioSource.PlayClipAtPoint(objectSounds[1], transform.position, 1f);
    }
    
    public void GetDamage()
    {
        MonsterInfo.LivesCount -= 1;
    }
    
    public void Walking()
    {
        if (Vector3.Distance(sprite.transform.position, targetPosition) > 0.1f && rb.isKinematic == false)
            MoveToTarget();
        else if (isHarassment == false)
            StartCoroutine(StopAndSetNewTarget());
    }

    public IEnumerator StopAndSetNewTarget()
    {
        isStopped = true;
        yield return new WaitForSeconds(2f);
        SetNewTargetPosition();
        isStopped = false;
    }

    public void SetNewTargetPosition()
    {
        var newX = Random.Range(-walkingRadius + SpritePosition.x, walkingRadius + SpritePosition.x);
        var newY = Random.Range(-walkingRadius + SpritePosition.y, walkingRadius + SpritePosition.y);
        targetPosition = new Vector3(newX, newY, sprite.transform.position.z);
    }
    
    private void Unfreeze()
    {
        rb.isKinematic = false;
        MoveToTarget();
        SetNewTargetPosition();
    }

    private void MoveToTarget()
    {
        sprite.transform.position = Vector3.MoveTowards(
            sprite.transform.position, targetPosition, MonsterInfo.CurrentSpeed * Time.deltaTime);   
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Zombie")) return;
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        Invoke(nameof(Unfreeze), freezeTime);
    }
}