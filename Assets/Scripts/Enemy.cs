using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Melee = 0,
    Ranged = 1,
}

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Spawnable spawnable;
    [SerializeField]
    private Health health;
    [SerializeField]
    private EnemyType enemyType;
    [SerializeField]
    private float rangeLimit = 2.5f;
    [SerializeField]
    private float meleeRange = 0.5f;
    [SerializeField]
    private float damagePerAttack = 1.0f;
    [SerializeField]
    private float rangedAttackRate = 0.5f;
    [SerializeField]
    private float moveSpeed = 0.5f;
    [SerializeField]
    private bool flipOnSpawn = true;

    private float rangedAttackTime = 0.0f;
    private bool alive;
    private PlayerManager playerManager;

    void Start()
    {
        Main main = Main.Instance;
        ManagerStore managerStore = main.ManagerStore;
        playerManager = managerStore.Get<PlayerManager>();
    }

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
            else // Perform enemy AI
            {
                Vector3 target = Vector3.zero;
                Vector3 pos = transform.position;
                Vector3 dir = (target - pos).normalized;

                float distance = Vector3.Distance(target, pos);

                if (enemyType == EnemyType.Melee) // Move towards melee range, hit and die
                {
                    if(distance < meleeRange) // Is the enemy close enough to damage the player?
                    {
                        // Instantly damage the player and kill self
                        playerManager.TakeDamage(damagePerAttack);
                        Die();
                    }
                    else
                    {
                        transform.position += moveSpeed * dir * Time.deltaTime;
                    }
                }
                else // Move towards near-circle, shoot until death
                {
                    rangedAttackTime += Time.deltaTime;
                    if(rangedAttackTime >= rangedAttackRate)
                    {
                        rangedAttackTime = 0.0f;
                        Shoot();
                    }
                    else
                    {
                        transform.position += moveSpeed * dir * Time.deltaTime;
                    }
                }
            }
        }
    }

    private void Shoot()
    {
        // Attack the player
        playerManager.TakeDamage(damagePerAttack);

        // SHOOT EFFECT HERE
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

        if(flipOnSpawn)
        {
            if (Random.Range(0, 10) < 5) // Roughly 50-50
            {
                Vector3 ls = transform.localScale;
                ls.x *= -1;
                transform.localScale = ls;
            }
        }
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
