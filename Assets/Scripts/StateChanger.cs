using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateChanger : MonoBehaviour
{
    public void OpenMenu()
    {
        Transition(GameState.Menu);
    }

    public void OpenGame()
    {
        Transition(GameState.Playing);
    }

    public void OpenDeath()
    {
        Transition(GameState.DeathScreen);
    }

    private void Transition(GameState state)
    {
        Main main = Main.Instance;
        ManagerStore managerStore = main.ManagerStore;
        StateManager stateManager = managerStore.Get<StateManager>();

        stateManager.SetState(state);
    }
}
