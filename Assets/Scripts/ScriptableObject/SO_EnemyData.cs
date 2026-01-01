using UnityEngine;

[CreateAssetMenu(fileName = "SO_EnemyData", menuName = "Scriptable Objects/SO_EnemyData")]
public class SO_EnemyData : ScriptableObject
{
    public string enemyName;
    public float expReward;
    public int scoreReward;
    public float damage;
}
