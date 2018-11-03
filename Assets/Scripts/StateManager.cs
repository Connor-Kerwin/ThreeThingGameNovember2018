using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : Manager, IMainListener
{
    private StateMachine<GameState> stateMachine;

    public GameState CurrentState { get { return stateMachine.Current; } }

    private void Awake()
    {
        stateMachine = new StateMachine<GameState>();
        SetState(GameState.Menu);

        Main main = Main.Instance;
        main.AddListener(this);
    }

    public override bool Link()
    {
        return true;
    }

    public void SetState(GameState state)
    {
        stateMachine.SetState(state);
    }

    public bool AddListener(IStateMachineListener<GameState> listener)
    {
        return stateMachine.AddListener(listener);
    }

    public bool RemoveListener(IStateMachineListener<GameState> listener)
    {
        return stateMachine.RemoveListener(listener);
    }

    public void OnFinishedInitialization(Main main)
    {
        SetState(GameState.Menu);
    }

    public void OnFinishedLink(Main main)
    {

    }
}