using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Health
{
    [SerializeField]
    private float maxHealth = 100.0f;
    [SerializeField]
    private float curHealth = 100.0f;

    public bool IsAlive { get { return curHealth > 0; } }
    public float CurrentHealth { get { return curHealth; } set { curHealth = value; } }
    public float MaxHealth { get { return maxHealth; } set { maxHealth = value; } }

    public void TakeDamage(float damage)
    {
        curHealth -= damage;
    }

    public void Heal(float health)
    {
        curHealth = Mathf.Clamp(health + curHealth, 0.0f, maxHealth);
    }

    public void Reset()
    {
        curHealth = maxHealth;
    }
}