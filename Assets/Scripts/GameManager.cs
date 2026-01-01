using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private Player player;
    public Player Player => player;
    public EnemyManager enemyManager;
    public ProjectileManager projectileManager;

    [Header("# Player Info")]
    public int killCount;

    [Header("# Stage Info")]
    public float timer = 600f;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        // 매 프레임 경과 시간을 차감 (결과적으로 1초에 1씩 감소)
        timer -= Time.deltaTime;

        if (timer < 0)
            timer = 0;
    }

    private void OnEnable()
    {
        // 정적 이벤트 구독 (Plan C)
        Enemy.OnEnemyDeath += HandleEnemyDeath;
    }

    private void OnDisable()
    {
        // 구독 해제 (메모리 누수 방지)
        Enemy.OnEnemyDeath -= HandleEnemyDeath;
    }

    private void HandleEnemyDeath(SO_EnemyData data)
    {
        killCount++;
        
        if (player != null && player.StatusInfo != null)
        {
            player.StatusInfo.GetExp(data.expReward);
        }
    }

    public Vector3 PlayerPosition => player != null ? player.transform.position : Vector3.zero;
}
