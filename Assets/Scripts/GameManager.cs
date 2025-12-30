using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private Player player;
    public Player Player => player;
    public EnemyManager enemyManager;
    public ProjectileManager projectileManager;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public Vector3 PlayerPosition => player != null ? player.transform.position : Vector3.zero;
}
