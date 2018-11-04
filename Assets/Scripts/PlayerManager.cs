using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Manager, IStateMachineListener<GameState>
{
    [SerializeField]
    private Health health;
    [SerializeField]
    private Transform player;

    private StateManager stateManager;

    public Health Health { get { return health; } }
    public Transform Player { get { return player; } }

    public void OnStateChanged(GameState previous, GameState current)
    {
        switch (current)
        {
            case GameState.Menu:
            case GameState.DeathScreen:
            case GameState.Playing:

            ResetHealth();

            break;
        }
    }

    public override bool Link()
    {
        Main main = Main.Instance;
        ManagerStore managerStore = main.ManagerStore;
        stateManager = managerStore.Get<StateManager>();
        stateManager.AddListener(this);

        return true;
    }

    public void ResetHealth()
    {
        health.Reset();
    }

    public void TakeDamage(float damage)
    {
        health.TakeDamage(damage);
        CheckHealth();
    }

    private void CheckHealth()
    {
        if(!health.IsAlive)
        {
            Debug.Log("PLAYER IS DEAD");

            // Transition to the death screen
            stateManager.SetState(GameState.DeathScreen);
        }
    }
}
