using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveEntryData
{
    [SerializeField]
    private string id;
    [SerializeField]
    private int count;

    public string ID { get { return id; } }
    public int Count { get { return count; } }

    public WaveEntryData(string id, int count)
    {
        this.id = id;
        this.count = count;
    }
}

[System.Serializable]
public class WaveEntry
{
    [SerializeField]
    private List<WaveEntryData> enemies;

    public List<WaveEntryData> Enemies { get { return enemies; } }
}

[System.Serializable]
public class WaveCache
{
    [SerializeField]
    private Queue<string> spawnQueue;
    [SerializeField]
    private List<Spawnable> activeList;

    public WaveCache()
    {
        spawnQueue = new Queue<string>();
        activeList = new List<Spawnable>();
    }

    public bool HasFinishedWave()
    {
        // Finished wave is when all objects have spawned and all active have despawned
        return spawnQueue.Count == 0 && activeList.Count == 0;
    }

    public bool GetNextEnemy(out Spawnable enemy)
    {
        if (spawnQueue.Count > 0) // Is there something to spawn?
        {
            string entry = spawnQueue.Dequeue();

            // Fetch the spawn manager
            Main main = Main.Instance;
            ManagerStore managerStore = main.ManagerStore;
            SpawnManager spawnManager = managerStore.Get<SpawnManager>();

            // Try to spawn the enemy
            if(spawnManager.Spawn(entry, out enemy))
            {
                activeList.Add(enemy);
                return true;
            }
            else // Enemy isn't valid
            {
                Debug.LogError("Wave manager unable to spawn " + entry + " because it's an invalid spawnable");
                return false;
            }
        }
        else // Nothing to spawn
        {
            enemy = null;
            return false;
        }
    }

    public void Cleanup()
    {
        // Fetch the spawn manager
        Main main = Main.Instance;
        ManagerStore managerStore = main.ManagerStore;
        SpawnManager spawnManager = managerStore.Get<SpawnManager>();

        // Iterate and despawn all objects
        foreach(Spawnable instance in activeList)
        {
            Despawn(spawnManager, instance);
        }
    }

    public void Clear()
    {
        spawnQueue.Clear();
    }

    private void Despawn(SpawnManager spawnManager, Spawnable instance)
    {
        // Store the manager for re-use
        spawnManager.Store(instance);

        // Move out of the scene
        instance.transform.position = new Vector3(0, -100, 0);
    }
    
    public void Populate(WaveEntry entry)
    {
        // Fetch the wave data and populate the spawn queue
        List<WaveEntryData> waveEntries = entry.Enemies;
        foreach(WaveEntryData data in waveEntries)
        {
            for(int i = 0; i < data.Count; i++) // Queue the number of enemies for the wave data
            {
                spawnQueue.Enqueue(data.ID);
            }
        }
    }
}

public class WaveManager : Manager
{
    [SerializeField]
    private List<WaveEntry> waves;
    [SerializeField]
    private float spawnRate = 0.1f;
    private float spawnTime = 0.0f;

    private int waveIndex;
    private WaveCache waveCache;

    public int MaxWaveIndex { get { return waves.Count - 1; } }

    private void Awake()
    {
        waveCache = new WaveCache();
    }

    private void Update()
    {
        spawnTime += Time.deltaTime;
        if(spawnTime >= spawnRate)
        {
            spawnTime = 0.0f;
            SpawnEnemies();
        }

        CheckForNextWave();
    }

    private void CheckForNextWave()
    {
        if(waveCache.HasFinishedWave())
        {
            NextWave();
        }
    }

    private void NextWave()
    {
        // consider a delay...

        waveIndex++;
        if(waveIndex > MaxWaveIndex)
        {
            waveIndex = 0;
        }

        // Start the wave
        StartWave();
    }

    private void SpawnEnemies()
    {
        Spawnable enemy;
        if(waveCache.GetNextEnemy(out enemy)) // Was an enemy found?
        {
            Vector3 pos = new Vector3(Random.Range(0, 10), 0, 0);
            enemy.transform.position = pos;
        }
    }

    private bool WaveIndexValid(int index)
    {
        return index >= 0 && index <= MaxWaveIndex;
    }

    private void CleanCurrentWave()
    {
        // Cleanup the wave cache and all active entries
        waveCache.Cleanup();

        // Remove all items to be spawned
        waveCache.Clear();
    }

    private void StartWave()
    {
        // Remove all items to be spawned
        waveCache.Clear();

        // Get the wave entry
        WaveEntry entry = waves[waveIndex];

        // Populate the wave cache with the items for the wave
        waveCache.Populate(entry);
    }

    public bool SetWave(int index)
    {
        // Ensure index is valid
        if(WaveIndexValid(index))
        {
            // Clean the current wave
            CleanCurrentWave();

            // Set the wave index
            waveIndex = index;

            // Start the wave
            StartWave();

            return true;
        }
        else // Invalid index
        {
            return false;
        }
    }
    
}
