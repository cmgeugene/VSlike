using System.Collections;
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
    private WaitForSeconds knockbackDuration = new WaitForSeconds(0.1f);

    public float Health { get; private set; } = 10f;
    public float maxHealth = 10f;
    public float speed = 1f;
    public float attackRange = 1.5f;
    public GameObject target;

    // 피격 관련 변수들
    bool isKnockback = false;
    bool isDead = false;


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
        // 초기화
        Health = maxHealth;
        isDead = false;
        body.enabled = true;
        rb.simulated = true;
        sprite.sortingOrder = 10;
        anim.SetBool("Dead", false);

        if (target == null)
        {
            target = GameManager.instance.Player.gameObject;
        }
    }

    IEnumerator Knockback()
    {
        isKnockback = true;
        Vector3 playerPos = GameManager.instance.PlayerPosition;
        Vector3 direction = (transform.position - playerPos).normalized;

        // 기존 속도를 초기화하여 넉백 힘이 일정하게 적용되도록 함
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction * 3f, ForceMode2D.Impulse);

        yield return knockbackDuration;

        rb.linearVelocity = Vector2.zero;
        isKnockback = false;
        // yield return null; 1프레임 쉬기
        // yield return new WaitForSeconds(2f); 2초 쉬기
    }
    
    public void TakeDamage(float damage)
    {
        // 이미 죽어서 풀에 반환 중인 적은 무시 (중복 Die 호출 방지)
        if (Health <= 0) return;

        Health -= damage;

        // 피격 : 피격 애니메이션 재생(트리거), 넉백(코루틴)
        if (Health <= 0)
        {
            isDead = true;
            body.enabled = false;
            rb.simulated = false;
            sprite.sortingOrder = 9;
            anim.SetBool("Dead", true);
            //Die();
        }
        else
        {
            anim.SetTrigger("Hit");
            StartCoroutine(Knockback());
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
        // 넉백 중이거나 타겟이 없으면 이동하지 않음
        if (isKnockback || isDead || target == null) return;

        Vector2 direction = (target.transform.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
        sprite.flipX = direction.x < 0;
    }

}
