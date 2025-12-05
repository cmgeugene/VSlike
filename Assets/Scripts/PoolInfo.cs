using UnityEngine;
[System.Serializable]
public class PoolInfo
{
    public string poolName;
    public GameObject prefab;
    public int defaultCapacity = 10;
    public int maxSize = 100;
}
