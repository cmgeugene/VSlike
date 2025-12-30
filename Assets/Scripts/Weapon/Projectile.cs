using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    private float damage;
    private int penetration;
    private float speed;
    private Vector2 direction;
    private IObjectPool<GameObject> myPool;
    private bool isReleased = false;

    public void Init(float damage, int penetration, float speed, Vector2 direction)
    {
        this.damage = damage;
        this.penetration = penetration;
        this.speed = speed;
        this.direction = direction;
        isReleased = false;

        if (direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        }
    }

    public void SetPool(IObjectPool<GameObject> pool)
    {
        myPool = pool;
    }

    void FixedUpdate()
    {
        transform.Translate(Vector3.up * speed * Time.fixedDeltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isReleased) return;

        // 다른 발사체와 충돌할 경우 무시
        if (other.CompareTag("Player") || other.GetComponent<Projectile>()!= null)
        {
            return;
        } 

        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            // 1. 관통 횟수 차감 및 제거 여부 먼저 결정
            penetration--;
            bool shouldRelease = penetration <= 0;

            // 2. 소멸해야 한다면 즉시 상태 변경 및 반환
            if (shouldRelease)
            {
                Release();
            }
            // 3. 데미지 처리 (적의 사망 로직에서 예외가 발생해도 발사체는 이미 처리됨)
            damageable.TakeDamage(damage);
        }
    }

    private void Release()
    {
        if (isReleased) return;
        isReleased = true;

        if (myPool != null) myPool.Release(gameObject);
        else Destroy(gameObject);
    }

    void OnBecameInvisible()
    {
        Release();
    }
}
