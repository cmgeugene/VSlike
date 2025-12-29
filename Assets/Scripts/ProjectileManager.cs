using Unity.VisualScripting;
using UnityEngine;

public class ProjectileManager : PoolManager
{
    public static ProjectileManager instance;

    protected override void Awake()
    {
        Debug.Log("Projectile Manager awake called");
        instance = this;
        base.Awake();
    }

    public new GameObject Get(string poolName)
    {
        GameObject obj = base.Get(poolName);
        if (obj == null) return null;

        Projectile projectile = obj.GetComponent<Projectile>();
        if(projectile != null)
        {
            projectile.SetPool(poolDictionary[poolName]);
        }

        return obj;
    }
}
