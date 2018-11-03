using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateMachineListener<T>
{
    void OnStateChanged(T previous, T current);
}

public class StateMachine<T>
{
    private T state;
    private List<IStateMachineListener<T>> listeners;

    public T Current { get { return state; } }

    public StateMachine()
    {
        listeners = new List<IStateMachineListener<T>>();
    }

    public bool AddListener(IStateMachineListener<T> listener)
    {
        if(!listeners.Contains(listener))
        {
            listeners.Add(listener);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool RemoveListener(IStateMachineListener<T> listener)
    {
        return listeners.Remove(listener);
    }

    public void SetState(T newState)
    {
        T previous = state;
        state = newState;

        foreach(IStateMachineListener<T> listener in listeners)
        {
            listener.OnStateChanged(previous, newState);
        }
    }
}
