using UnityEngine;
using System.Collections;

public class PoolTester : MonoBehaviour
{
    public EnemyManager enemyManager;
    public ProjectileManager projectileManager;

    void Update()
    {
        // Press Z to spawn Zombie
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (enemyManager != null)
            {
                var enemy = enemyManager.Get("Zombie");
                if (enemy != null)
                {
                    Debug.Log($"Spawned Zombie: {enemy.name}");
                    StartCoroutine(ReleaseAfterTime(enemyManager, enemy, 2f));
                }
            }
            else
            {
                Debug.LogWarning("EnemyManager is not assigned!");
            }
        }

        // Press X to spawn Skeleton
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (enemyManager != null)
            {
                var enemy = enemyManager.Get("Skeleton");
                if (enemy != null)
                {
                    Debug.Log($"Spawned Skeleton: {enemy.name}");
                    StartCoroutine(ReleaseAfterTime(enemyManager, enemy, 2f));
                }
            }
            else
            {
                Debug.LogWarning("EnemyManager is not assigned!");
            }
        }

        // Press P to spawn Projectile
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (projectileManager != null)
            {
                var proj = projectileManager.Get();
                Debug.Log($"Spawned Projectile: {proj.name}");
                StartCoroutine(ReleaseAfterTime(projectileManager, proj, 2f));
            }
            else
            {
                Debug.LogWarning("ProjectileManager is not assigned!");
            }
        }
    }

    IEnumerator ReleaseAfterTime(PoolManager pool, GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        pool.Release(obj);
        Debug.Log($"Released: {obj.name}");
    }
}
