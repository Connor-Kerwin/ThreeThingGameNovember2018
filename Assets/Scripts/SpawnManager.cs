using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface ISpawnEntry
{
    Spawnable Prefab { get; }
}

public class SpawnEntry<T> : ISpawnEntry where T : Spawnable
{
    private Spawnable prefab;
    private Pool<T> pool;

    public Spawnable Prefab { get { return prefab; } }

    public SpawnEntry(Spawnable prefab, Func<T> create)
    {
        this.prefab = prefab;
        pool = new Pool<T>(create);
    }

    public T Get()
    {
        return pool.Get();
    }

    public void Store(T item)
    {
        pool.Store(item);
    }
}

public class SpawnStore
{
    private List<ISpawnEntry> entries;

    public SpawnStore()
    {
        entries = new List<ISpawnEntry>();
    }

    public void AddStore<T>(Spawnable prefab, Func<T> create) where T : Spawnable
    {
        // Try to get the store before adding a new one
        SpawnEntry<T> spawnEntry = GetStore<T>();
        if(spawnEntry == null)
        {
            // Create the store and get it
            spawnEntry = CreateStore<T>(prefab, create);
        }
    }

    public SpawnEntry<T> GetStore<T>() where T : Spawnable
    {
        return GetStore<T>(typeof(T));
    }

    private SpawnEntry<T> GetStore<T>(Type type) where T : Spawnable
    {
        // Iterate all entries
        foreach(ISpawnEntry entry in entries)
        {
            if(entry.GetType() == type) // Check for a matching type
            {
                return entry as SpawnEntry<T>;
            }
        }

        return null;
    }

    private SpawnEntry<T> CreateStore<T>(Spawnable prefab, Func<T> create) where T : Spawnable
    {
        SpawnEntry<T> spawnEntry = new SpawnEntry<T>(prefab, create);
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

        }

        return flag;
    }

    public void Register<T>(Spawnable prefab, Func<T> create) where T : Spawnable
    {
        spawnStore.AddStore<T>(prefab, create);
    }

    public bool Spawn<T>(out T item) where T : Spawnable
    {
        SpawnEntry<T> entry = spawnStore.GetStore<T>();
        if(entry != null) // Was the entry found
        {
            item = entry.Get();
            return true;
        }
        else // Entry not found
        {
            item = null;
            return false;
        }
    }

    public bool Store<T>(T item) where T : Spawnable
    {
        SpawnEntry<T> entry = spawnStore.GetStore<T>();
        if (entry != null) // Was the entry found
        {
            entry.Store(item);
            return true;
        }
        else // Entry not found
        {
            return false;
        }
    }
}