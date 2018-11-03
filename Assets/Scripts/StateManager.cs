using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : Manager
{
    private StateMachine<GameState> stateMachine;

    public GameState CurrentState { get { return stateMachine.Current; } }

    private void Awake()
    {
        stateMachine = new StateMachine<GameState>();
        SetState(GameState.Menu);
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
}