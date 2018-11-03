using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnArgs : EventArgs
{
    private Spawnable spawnable;

    public Spawnable Spawnable { get { return spawnable; } }

    public SpawnArgs(Spawnable spawnable)
    {
        this.spawnable = spawnable;
    }
}

public class Spawnable : MonoBehaviour {

    [SerializeField]
    private string id;

    public EventHandler<SpawnArgs> OnDespawn;
    public EventHandler<SpawnArgs> OnSpawn;

    public string ID { get { return id; } }

    public void Spawn()
    {
        if (OnSpawn != null)
        {
            OnSpawn.Invoke(this, new SpawnArgs(this));
        }
    }

    public void Despawn()
    {
        if(OnDespawn != null)
        {
            OnDespawn.Invoke(this, new SpawnArgs(this));
        }
    }
}
