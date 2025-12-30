using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] 
    protected List<PoolInfo> poolList;
    protected Dictionary<string, IObjectPool<GameObject>> poolDictionary;
    protected Dictionary<string, PoolInfo> poolInfoDictionary;
    protected IObjectPool<GameObject> pool;

    protected virtual void Awake()
    {
        InitPools();
    }

    protected void InitPools()
    {
        poolDictionary = new Dictionary<string, IObjectPool<GameObject>>();
        poolInfoDictionary = new Dictionary<string, PoolInfo>();

        foreach (var info in poolList)
        {
            PoolInfo currentInfo = info;
            poolInfoDictionary.Add(currentInfo.poolName, currentInfo);

            IObjectPool<GameObject> newPool = new ObjectPool<GameObject>(
                
                createFunc: () => 
                {
                    // currentInfo.prefab 캡쳐
                    GameObject obj = Instantiate(currentInfo.prefab,transform);
                    obj.name = currentInfo.poolName + "_Pooled"; // 디버깅용 이름 변경
                    
                    return obj;
                },
                actionOnGet: (obj) =>
                {
                    OnTakeFromPool(obj);
                    currentInfo.activeCount++;
                },
                actionOnRelease: (obj) =>
                {
                    OnReturnToPool(obj);
                    currentInfo.activeCount = Mathf.Max(0, currentInfo.activeCount - 1);
                },
                actionOnDestroy: OnDestroyPoolObject, 
                collectionCheck: true,
                defaultCapacity: currentInfo.defaultCapacity,
                maxSize: currentInfo.maxSize
            );

            poolDictionary.Add(currentInfo.poolName, newPool);
        }
    }
    protected virtual void OnTakeFromPool(GameObject obj)
    {
        obj.SetActive(true);
    }

    protected virtual void OnReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
    }

    protected virtual void OnDestroyPoolObject(GameObject obj)
    {
        Destroy(obj);
    }

    public GameObject Get(string poolName)
    {
        // 최대 활성화 수 체크 먼저
        if(poolInfoDictionary[poolName].activeCount >= poolInfoDictionary[poolName].maxActiveCount)
        {
            return null;
        }
        
        return poolDictionary[poolName].Get();
    }
}
