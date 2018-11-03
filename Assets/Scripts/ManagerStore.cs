using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerStore : IEnumerable<Manager>
{
    private List<Manager> managers;
    public ManagerStore()
    {
        managers = new List<Manager>();
    }

    public void Add(Manager manager)
    {
        if(!managers.Contains(manager))
        {
            managers.Add(manager);
        }
    }
    
    public T Get<T>() where T : Manager
    {
        Type mType = typeof(T);
        foreach(Manager manager in managers)
        {
            if(manager.GetType() == mType)
            {
                return manager as T;
            }
        }

        return null;
    }

    private StandardEnumerator<Manager> GetStandardEnumerator()
    {
        return new StandardEnumerator<Manager>(managers);
    }

    public IEnumerator<Manager> GetEnumerator()
    {
        return GetStandardEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetStandardEnumerator();
    }
}
