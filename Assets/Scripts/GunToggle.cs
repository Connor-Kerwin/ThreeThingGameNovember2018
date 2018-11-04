using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunToggle : MonoBehaviour, IStateMachineListener<GameState>
{
    [SerializeField]
    private Transform weapon;

    public void OnStateChanged(GameState previous, GameState current)
    {
        switch (current)
        {
            case GameState.Menu:
            case GameState.DeathScreen:

                weapon.gameObject.SetActive(false);

                break;
            case GameState.Playing:

                weapon.gameObject.SetActive(true);
                break;
        }
    }

    // Use this for initialization
    void Start ()
    {
        Main main = Main.Instance;
        ManagerStore managerStore = main.ManagerStore;
        StateManager stateManager = managerStore.Get<StateManager>();
        stateManager.AddListener(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
