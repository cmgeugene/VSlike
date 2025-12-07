using UnityEngine;

[CreateAssetMenu(fileName = "SO_WeaponData", menuName = "Scriptable Objects/SO_WeaponData")]
public class SO_WeaponData : ScriptableObject
{
    public int weaponId;
    public string weaponName;
    
    public WeaponData[] levelData;
}
