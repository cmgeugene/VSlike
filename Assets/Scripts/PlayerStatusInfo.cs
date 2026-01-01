using UnityEngine;
using System;

public class PlayerStatusInfo : MonoBehaviour
{
    [Header("# Health Info")]
    public float health;
    public float maxHealth = 100f;
    public event Action<float, float> OnHealthChanged;

    [Header("# Level Info")]
    public int level = 1;
    public float exp;
    public float[] nextExp = { 10, 30, 60, 100, 150, 210, 280, 360, 450, 600 };
    public event Action<float, float> OnExpChanged;
    public event Action<int> OnLevelChanged;

    void Start()
    {
        health = maxHealth;
        OnHealthChanged?.Invoke(health, maxHealth);
        OnExpChanged?.Invoke(exp, nextExp[level]);
        OnLevelChanged?.Invoke(level);
    }

    public void ApplyDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            Debug.Log("플레이어 사망!");
        }
        OnHealthChanged?.Invoke(health, maxHealth);
    }

    public void GetExp(float amount)
    {
        exp += amount;

        while (level < nextExp.Length && exp >= nextExp[level])
        {
            exp -= nextExp[level];
            level++;
            OnLevelChanged?.Invoke(level);
        }
        
        float currentNextExp = (level < nextExp.Length) ? nextExp[level] : 0;
        OnExpChanged?.Invoke(exp, currentNextExp);
    }
}
