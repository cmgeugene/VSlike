using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] Movement movement; // 인스펙터에서 할당하거나 자동으로 가져오도록 설정
    public Movement Movement => movement;
    
    [SerializeField] PlayerStatusInfo statusInfo;
    public PlayerStatusInfo StatusInfo => statusInfo;

    void Awake()
    {
        if (movement == null) movement = GetComponent<Movement>();
        if (statusInfo == null) statusInfo = GetComponent<PlayerStatusInfo>();
    }

    public void TakeDamage(float damage, Vector3 attackerPosition)
    {
        if (statusInfo != null)
        {
            statusInfo.ApplyDamage(damage);
            
            // 이후 attackPosition을 이용하여 물리 처리
        }
    }
}
