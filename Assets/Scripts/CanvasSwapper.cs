using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSwapper : MonoBehaviour, IStateMachineListener<GameState>, IMainListener
{
    [SerializeField]
    private Canvas menuCanvas;
    [SerializeField]
    private Canvas gameCanvas;
    [SerializeField]
    private Canvas deathCanvas;

    void Awake()
    {
        // Disable canvases initially
        menuCanvas.gameObject.SetActive(false);
        gameCanvas.gameObject.SetActive(false);
        deathCanvas.gameObject.SetActive(false);

        Main main = Main.Instance;
        main.AddListener(this);
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

    public void OnFinishedInitialization(Main main)
    {

    }

    public void OnFinishedLink(Main main)
    {
        // Self-register for state updates
        ManagerStore managerStore = main.ManagerStore;
        StateManager stateManager = managerStore.Get<StateManager>();
        stateManager.AddListener(this);
    }
}
