using System.Collections;
using UnityEngine;

public interface IMonster
{
    int LivesCount { get; set; }
    float StandardSpeed { get; set; }
    float WalkingRadius { get; set; }
    Vector3 SpritePosition { get; set; }

    void Awake();
    void LateUpdate();
    void Die();
    void GetDamage();
    void Walking();
    IEnumerator StopAndSetNewTarget();
    void SetNewTargetPosition();
}

public class Monster : MonoBehaviour, IMonster
{
    public int LivesCount { get; set; }
    public float StandardSpeed { get; set; }
    public float WalkingRadius { get; set; }
    public Vector3 SpritePosition { get; set; }

    private Collider2D collider;
    private SpriteRenderer sprite;
    private Vector3 targetPosition;
    private bool isStopped = false;
    public LayerMask obstacleLayer;
    private Rigidbody2D rb;
    public float freezeTime = 2f;

    public virtual void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        SpritePosition = sprite.transform.position;
        SetNewTargetPosition();
    }

    public virtual void LateUpdate()
    {
        if (isStopped)
            StartCoroutine(StopAndSetNewTarget());
        Walking();
    }

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

    public virtual void Walking()
    {
        if (Vector3.Distance(sprite.transform.position, targetPosition) > 0.1f && rb.isKinematic == false)
        {
            Vector3 direction = (targetPosition - sprite.transform.position).normalized;
            sprite.transform.position = Vector3.MoveTowards(sprite.transform.position, targetPosition, StandardSpeed * Time.deltaTime);
        }
        else
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

    public void SetNewTargetPosition()
    {
        var newX = Random.Range(-WalkingRadius + SpritePosition.x, WalkingRadius + SpritePosition.x);
        var newY = Random.Range(-WalkingRadius + SpritePosition.y, WalkingRadius + SpritePosition.y);
        targetPosition = new Vector3(newX, newY, sprite.transform.position.z);
    }

    private void Unfreeze()
    {
        // Размораживаем объект
        rb.isKinematic = false;
        Vector3 direction = -(targetPosition - sprite.transform.position).normalized;
        sprite.transform.position = Vector3.MoveTowards(sprite.transform.position, targetPosition, StandardSpeed * Time.deltaTime);
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
}