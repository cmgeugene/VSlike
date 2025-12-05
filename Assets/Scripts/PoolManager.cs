using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] List<PoolInfo> poolList;
    private Dictionary<string, IObjectPool<GameObject>> poolDictionary;
    protected IObjectPool<GameObject> pool;

    protected virtual void Awake()
    {
        InitPools();
    }

    protected void InitPools()
    {
        poolDictionary = new Dictionary<string, IObjectPool<GameObject>>();
        
        foreach (var info in poolList)
        {
            PoolInfo currentInfo = info;
            IObjectPool<GameObject> newPool = new ObjectPool<GameObject>(
                
                createFunc: () => 
                {
                    // currentInfo.prefab 캡쳐
                    GameObject obj = Instantiate(currentInfo.prefab);
                    obj.name = currentInfo.poolName + "_Pooled"; // 디버깅용 이름 변경

                    // 생성된 오브젝트에게 "너네 집(Pool)은 여기야"라고 알려주기 (Self-Return 패턴)
                    // *팁: 딕셔너리에 넣기 전이라 'newPool' 변수를 직접 쓰기도 애매합니다.
                    // 보통은 여기서 SetPool을 안 하고, 아래 Get() 함수에서 꺼내줄 때 SetPool을 합니다.
                    
                    return obj;
                },
                actionOnGet: OnTakeFromPool, 
                actionOnRelease: OnReturnToPool, 
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
        if(poolDictionary.ContainsKey(poolName))
        {
            GameObject obj = poolDictionary[poolName].Get();
            // 여기서 오브젝트에게 자기 풀을 알려줍니다. 
            // 예시: BaseEnemy 컴포넌트가 있다면 찾아서 주입
            // BaseEnemy enemy = obj.GetComponent<BaseEnemy>();
            // if (enemy != null) enemy.SetPool(poolDictionary[poolName]);

            Enemy enemy = obj.GetComponent<Enemy>();
            if(enemy != null) enemy.SetPool(poolDictionary[poolName]);
            return obj;
        }

        Debug.LogError($"PoolManager: {poolName} 이름의 풀은 없습니다.");
        return null;
    }
}
