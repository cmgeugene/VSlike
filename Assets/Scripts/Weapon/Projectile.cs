using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    private float damage;
    private int penetration;
    private float speed;
    private Vector2 direction;
    private IObjectPool<GameObject> myPool;

    public void Init(float damage, int penetration, float speed, Vector2 direction)
    {
        this.damage = damage;
        this.penetration = penetration;
        this.speed = speed;
        this.direction = direction;

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
        // 다른 발사체와 충돌할 경우 무시
        if (other.CompareTag("Player") || other.GetComponent<Projectile>()!= null)
        {
            Debug.Log("다른 발사체와 충돌!");
            return;
        } 

        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
            if (penetration > 0)
            {
                penetration--;
                if(penetration == 0)
                {
                    Release();
                }
            }
        }
    }

    private void Release()
    {
        if(myPool != null) myPool.Release(gameObject);
        else Destroy(gameObject);
    }

    void OnBecameInvisible()
    {
        Release();
    }
}
