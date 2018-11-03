using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface ISpawnEntry
{
    Spawnable Prefab { get; }
}

public class SpawnEntry
{
    private Spawnable prefab;
    private Pool<Spawnable> pool;
    
    public string ID { get { return prefab.ID; } }

    public SpawnEntry(Spawnable prefab)
    {
        this.prefab = prefab;
        pool = new Pool<Spawnable>(Spawn);
    }

    private Spawnable Spawn()
    {
        Spawnable instance = GameObject.Instantiate<Spawnable>(prefab);
        return instance;
    }

    public Spawnable Get()
    {
        return pool.Get();
    }

    public void Store(Spawnable item)
    {
        pool.Store(item);
    }
}

public class SpawnStore
{
    private List<SpawnEntry> entries;

    public SpawnStore()
    {
        entries = new List<SpawnEntry>();
    }

    public bool AddStore(Spawnable prefab)
    {
        // Try to get the store before adding a new one
        SpawnEntry spawnEntry;
        if (!GetStore(prefab.ID, out spawnEntry))
        {
            spawnEntry = CreateStore(prefab);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool GetStore(string id, out SpawnEntry outEntry)
    {
        // Iterate all entries
        foreach(SpawnEntry entry in entries)
        {
            if(entry.ID == id) // Check for a matching ID
            {
                outEntry = entry;
                return true;
            }
        }

        outEntry = null;
        return false;
    }

    private SpawnEntry CreateStore(Spawnable prefab)
    {
        SpawnEntry spawnEntry = new SpawnEntry(prefab);
        entries.Add(spawnEntry);
        return spawnEntry;
    }
}

public class SpawnManager : Manager
{
    [SerializeField]
    private List<Spawnable> prefabs;

    private SpawnStore spawnStore;

    private void Awake()
    {
        spawnStore = new SpawnStore();
    }

    public override bool Initialize()
    {
        return RegisterPrefabs();
    }

    private bool RegisterPrefabs()
    {
        bool flag = true;

        foreach(Spawnable spawnable in prefabs)
        {
            if(Register(spawnable)) // Catch successful registration
            {
                Debug.Log("Registered spawnable " + spawnable.ID);
            }
            else // Catch failure
            {
                Debug.LogError("Failed to register spawnable " + spawnable.ID);
                flag = false;
            }
        }

        return flag;
    }

    public bool Register(Spawnable prefab)
    {
        return spawnStore.AddStore(prefab);
    }

    public bool Spawn(string id, out Spawnable item)
    {
        SpawnEntry entry;
        if(spawnStore.GetStore(id, out entry))
        {
            item = entry.Get();
            return true;
        }
        else // Store not found for given ID
        {
            item = null;
            return false;
        }
    }

    public bool Store(Spawnable item)
    {
        SpawnEntry entry;
        if (spawnStore.GetStore(item.ID, out entry))
        {
            entry.Store(item);
            return true;
        }
        else // Store not found for given item
        {
            return false;
        }
    }
}