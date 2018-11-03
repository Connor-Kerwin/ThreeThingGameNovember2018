using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Spawnable spawnable;
    [SerializeField]
    private Health health;

    private bool alive;

    private void OnCargoDetached(object parent)
    {
        alive = true;
        Spawn();
    }

    private void Update()
    {
        if(alive)
        {
            // Has the enemy died?
            if (!AliveCheck())
            {
                Die();
            }

            // Move towards player...
        }
    }

    public void TakeDamage(float damage)
    {
        health.TakeDamage(damage);

        // Has the enemy died?
        if(!AliveCheck())
        {
            Die();
        }
    }

    public void Spawn()
    {
        alive = true;
        health.Reset();
    }

    private void Die()
    {
        alive = false;

        // Fetch the spawn manager
        Main main = Main.Instance;
        ManagerStore managerStore = main.ManagerStore;
        SpawnManager spawnManager = managerStore.Get<SpawnManager>();

        // Store the enemy
        spawnManager.Store(spawnable);
    }

    private bool AliveCheck()
    {
        return health.IsAlive;
    }
}
