using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Player player;
    public EnemyManager enemyManager;
    public ProjectileManager projectileManager;

    void Awake()
    {
        instance = this;
    }

}
