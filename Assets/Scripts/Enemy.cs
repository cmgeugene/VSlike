using System;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class Enemy : MonoBehaviour, IDamageable
{

    private IObjectPool<GameObject> myPool;
    public void SetPool(IObjectPool<GameObject> Pool)
    {
        myPool = Pool;
    }

    public void Die()
    {
        if(myPool != null)
        {
            myPool.Release(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    protected Rigidbody2D rb;
    protected Animator anim;
    protected SpriteRenderer sprite;
    protected CapsuleCollider2D body;

    public float Health { get; private set; } = 10f;
    public float maxHealth = 10f;
    public float speed = 1f;
    public float attackRange = 1.5f;
    public GameObject target;


    protected void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Attack();
        }
    }

    protected void Awake()
    {
        if(!target)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
        
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        body = GetComponent<CapsuleCollider2D>();

        CreateAttackRange();
    }

    protected void OnEnable()
    {
        Health = maxHealth;
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
    }
    
    public void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
    }
    
    protected  void CreateAttackRange()
    {
        GameObject rangeObj = new GameObject("Attack Range");
        rangeObj.transform.parent = transform;
        rangeObj.transform.localPosition = Vector3.zero;

        CircleCollider2D col = rangeObj.AddComponent<CircleCollider2D>();
        col.isTrigger = true;
        col.radius = attackRange;
    }

    protected void FixedUpdate()
    {
        Move(target);
    }
    protected void Attack()
    {
        Debug.Log("공격");
    }

    protected void Init()
    {
        
    }

    protected void Move(GameObject target)
    {
        Vector2 direction = (target.transform.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
        sprite.flipX = direction.x < 0 ? true : false;
    }

}
