using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Monster : MonoBehaviour
{
    protected int livesCount;
    protected Collider2D collider;
    protected float standardSpeed;
    protected float walkingRadius;
    protected SpriteRenderer sprite;
    private Vector3 targetPosition;
    private bool isStopped = true;
    public LayerMask obstacleLayer;

    public int LivesCount
    {
        get { return livesCount; }
        set { livesCount = value; }
    }

    public float StandardSpeed
    {
        get { return standardSpeed; }
        set { standardSpeed = value; }
    }

    public float WalkingRadius
    {
        get { return walkingRadius; }
        set { walkingRadius = value; }
    }
    private float attackRadius
     {
        get { return attackRadius; }
        set { attackRadius = value; }
    }
    
    [SerializeField] Vector3 spritePosition {get; set;}

    public virtual void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        spritePosition = sprite.transform.position;
        SetNewTargetPosition();
        
    }

    public virtual void Update()
    {
        Walking();
    }

    public virtual void Die()
    {
        Destroy(gameObject, 1.4f);   
    }

    public virtual void GetDamage()
    {
        livesCount -= 1;
        if (livesCount == 0)
            Die();
    }

    public virtual void Walking()
    {
        if (Vector3.Distance(sprite.transform.position, targetPosition) > 0.001f && isStopped)
        {
            Vector3 direction = (targetPosition - sprite.transform.position).normalized;
            sprite.transform.position = Vector3.MoveTowards(sprite.transform.position, targetPosition, StandardSpeed * Time.deltaTime);
        }
        else if (!isStopped)
        {
            isStopped = true;
            StartCoroutine(StopForSeconds());
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Этот метод будет вызван, когда другой коллайдер входит в столкновение с myCollider
        isStopped = false;
        SetNewTargetPosition();
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        // Этот метод будет вызван, когда другой коллайдер входит в столкновение с myCollider
        isStopped = false;
        SetNewTargetPosition();
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        SetNewTargetPosition();        
    }
    

    private IEnumerator StopForSeconds()
    {
        yield return new WaitForSeconds(2f); // Остановка на 2 секунды
        isStopped = false;
        SetNewTargetPosition();
    }


    private void SetNewTargetPosition()
    {
        var newX = Random.Range(-walkingRadius + spritePosition.x, walkingRadius + spritePosition.x);
        var newY = Random.Range(-walkingRadius + spritePosition.y, walkingRadius + spritePosition.y);
        targetPosition = new Vector3(newX, newY, sprite.transform.position.z);
    }
}
