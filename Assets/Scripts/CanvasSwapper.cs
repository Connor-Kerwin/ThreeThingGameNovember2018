using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSwapper : MonoBehaviour, IStateMachineListener<GameState>
{
    [SerializeField]
    private Canvas menuCanvas;
    [SerializeField]
    private Canvas gameCanvas;
    [SerializeField]
    private Canvas deathCanvas;

    void Start()
    {
        // Self-register for state updates
        Main main = Main.Instance;
        ManagerStore managerStore = main.ManagerStore;
        StateManager stateManager = managerStore.Get<StateManager>();
        stateManager.AddListener(this);
        
        // Disable canvases initially
        menuCanvas.gameObject.SetActive(false);
        gameCanvas.gameObject.SetActive(false);
        deathCanvas.gameObject.SetActive(false);
    }

    public void OnStateChanged(GameState previous, GameState current)
    {
        Canvas disable = GetCanvasForState(previous);
        disable.gameObject.SetActive(false);

        Canvas enable = GetCanvasForState(current);
        enable.gameObject.SetActive(true);
    }

    private Canvas GetCanvasForState(GameState state)
    {
        switch (state)
        {
            case GameState.Menu:
                return menuCanvas;
            case GameState.Playing:
                return gameCanvas;
            case GameState.DeathScreen:
                return deathCanvas;
            default:
                return gameCanvas;
        }
    }
}
