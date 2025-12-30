using System.Collections;
using UnityEngine;

public class EnemyManager : PoolManager
{
    public EnemyManager enemyManager;
    public Transform playerTransform;

    protected override void Awake()
    {
        base.Awake();
        enemyManager = this;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public new GameObject Get(string poolName)
    {
        GameObject obj = base.Get(poolName);
        if (obj == null) return null;

        Enemy enemy = obj.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.SetPool(poolDictionary[poolName]);
        }
        return obj;
    }

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while(true)
        {
            Vector2 spawnPosition = (Vector2)playerTransform.position + Random.insideUnitCircle.normalized * 10f;

            float random = Mathf.Round(Random.Range(0f, 1f));
            string enemyName = random == 0 ? "Zombie" : "Skeleton";
            GameObject enemyObj = Get(enemyName);
            if(enemyObj != null)
            {
                enemyObj.transform.position = spawnPosition;
                // 단계적인 강화를 원한다면 여기서
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
}
