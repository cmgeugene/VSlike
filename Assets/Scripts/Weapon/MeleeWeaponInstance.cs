using UnityEngine;

public class MeleeWeaponInstance : MonoBehaviour
{
    // 컨트롤러가 이 값을 설정해 줄 것입니다.
    public float damage;

    void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            // 자신의 damage 값을 사용하여 피해를 줍니다.
            damageable.TakeDamage(damage, transform.position);
        }
    }
}
