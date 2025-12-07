using UnityEngine;
using UnityEngine.InputSystem;

public abstract class BaseWeapon : MonoBehaviour
{
    public SO_WeaponData weaponDataSO;

    protected int level = 1;
    
    protected int damage;
    protected int count;
    protected float range;
    protected float speed;
    protected float fireRate;
    protected string description;

    protected virtual void Awake()
    {
        ApplyStats(1);
    }

    protected virtual void Update()
    {
                // for test
        if(Keyboard.current.kKey.wasPressedThisFrame)
        {
            LevelUp();
        }
    }

    protected virtual void FixedUpdate()
    {

    }

    public void LevelUp()
    {
        level++;
        ApplyStats(level);
        Debug.Log("무기 레벨 업!");

        if(this is MeleeWeapon)
        {
            
            
        }
    }

    protected abstract void Attack();

    protected virtual void ApplyStats(int level)
    {
        WeaponData data = weaponDataSO.levelData[level];
        this.damage = data.damage;
        this.count = data.count;
        this.range = data.range;
        this.speed = data.speed;
        this.fireRate = data.fireRate;
        this.description = data.description;
    }
}
