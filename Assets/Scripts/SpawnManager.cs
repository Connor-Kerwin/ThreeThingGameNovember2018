﻿using System.Collections;
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
        pool.PreWarm(5, Prewarm);
    }

    private void Prewarm(Spawnable item)
    {
        item.transform.position = new Vector3(0, -100, 0);
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

    private List<Spawnable> active;
    private SpawnStore spawnStore;

    public List<Spawnable> Active { get { return active; } }

    public List<Spawnable> GetActiveCopy()
    {
        List<Spawnable> copy = new List<Spawnable>();
        foreach(Spawnable s in active)
        {
            copy.Add(s);
        }

        return copy;
    }

    private void Awake()
    {
        spawnStore = new SpawnStore();
        active = new List<Spawnable>();
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
            else // Catch failure through duplicate entry
            {
                Debug.LogError("Failed to register spawnable, duplicate entry is " + spawnable.ID);
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
        if(spawnStore.GetStore(id, out entry)) // Store found
        {
            item = entry.Get();
            item.Spawn();
            active.Add(item);
            return true;
        }
        else // Store not found for given ID
        {
            item = null;
            return false;
        }
    }

    public bool SpawnAt(string id, Vector3 position, out Spawnable item)
    {
        return SpawnAt(id, position, Quaternion.Euler(Vector3.zero), out item);
    }

    public bool SpawnAt(string id, Vector3 position, Vector3 rotation, out Spawnable item)
    {
        return SpawnAt(id, position, Quaternion.Euler(rotation), out item);
    }

    public bool SpawnAt(string id, Vector3 position, Quaternion rotation, out Spawnable item)
    {
        if(Spawn(id, out item))
        {
            item.transform.position = position;
            item.transform.rotation = rotation;
            return true;
        }
        else // Failure to spawn
        {
            return false;
        }
    }

    public bool Store(Spawnable item)
    {
        SpawnEntry entry;
        if (spawnStore.GetStore(item.ID, out entry)) // Store found
        {
            // NOTE: Possible bug where an object is stored twice?

            item.transform.position = new Vector3(0, -100, 0);
            item.Despawn();
            entry.Store(item);
            active.Remove(item);
            return true;
        }
        else // Store not found for given item
        {
            return false;
        }
    }
}