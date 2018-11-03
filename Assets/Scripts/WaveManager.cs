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

    public WaveCache()
    {
        spawnQueue = new Queue<string>();
    }

    public bool HasFinishedSpawning()
    {
        return spawnQueue.Count == 0;
    }

    //private void OnDespawn(object sender, SpawnArgs e)
    //{
    //    // Remove the spawnable and unregister from it
    //    activeList.Remove(e.Spawnable);
    //    e.Spawnable.OnDespawn -= OnDespawn;
    //}

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
                // Register to the spawnables despawn
                //enemy.OnDespawn += OnDespawn;
                //activeList.Add(enemy);
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
        //int count = activeList.Count;
        //for(int i = 0; i < activeList.Count; i++)
        //{
        //    Spawnable spawnable = activeList[0];
        //    Despawn(spawnManager, spawnable);
        //}

        //Debug.Log("Cleaned up " + count);

        //foreach(Spawnable instance in activeList)
        //{
        //    Despawn(spawnManager, instance);
        //}
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

public class WaveManager : Manager, IStateMachineListener<GameState>
{
    [SerializeField]
    private List<WaveEntry> waves;
    [SerializeField]
    private float spawnRate = 0.1f;
    [SerializeField]
    private int enemiesPerCycle = 2;
    [SerializeField]
    private float spawnRadius = 2.5f;
    [SerializeField]
    private float spawnHeight = 2.0f;

    private bool alive;
    private float spawnTime = 0.0f;
    private int waveIndex;
    private WaveCache waveCache;
    private PodManager podManager;
    private SpawnManager spawnManager;

    public int MaxWaveIndex { get { return waves.Count - 1; } }

    private void Awake()
    {
        waveCache = new WaveCache();
        alive = false;
    }

    private void Update()
    {
        if (alive)
        {
            spawnTime += Time.deltaTime;
            if (spawnTime >= spawnRate)
            {
                spawnTime = 0.0f;
                SpawnEnemies();
            }

            CheckForNextWave();
        }
    }

    private void CheckForNextWave()
    {
        // Check whether the cache has no more items to spawn and all active are destroyed
        if(waveCache.HasFinishedSpawning() && !DoEnemiesRemain())
        {
            NextWave();
        }
    }

    private bool DoEnemiesRemain()
    {
        List<Spawnable> active = spawnManager.Active;

        foreach (Spawnable s in active)
        {
            if (s.SpawnType == SpawnType.Enemy)
            {
                return true;
            }
        }

        return false;
    }

    private void RemoveActiveEnemies()
    {
        List<Spawnable> active = spawnManager.Active;
        int count = active.Count;
        for(int i = count-1; i >= 0; i--) // Backwards iteration due to deletion
        {
            Spawnable s = active[i];
            if(s.SpawnType == SpawnType.Enemy)
            {
                spawnManager.Store(s);
            }
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
        for (int i = 0; i < enemiesPerCycle; i++)
        {
            Spawnable enemy;
            if (waveCache.GetNextEnemy(out enemy)) // Was an enemy found?
            {
                Vector3 pos = GetSpawnPosition();
                enemy.transform.position = pos;

                // Spawn a pod for the enemy
                Pod pod = podManager.SpawnPod(pos, enemy.transform);
                pod.Launch(pos);
            }
        }
    }

    private Vector3 GetSpawnPosition()
    {
        float randomAngle = Random.Range(0.0f, 360.0f);
        float x = spawnRadius * Mathf.Cos(randomAngle);
        float y = spawnHeight;
        float z = spawnRadius * Mathf.Sin(randomAngle);

        return new Vector3(x, y, z);
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

        // Force remove all active enemies
        RemoveActiveEnemies();
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

    public override bool Link()
    {
        Main main = Main.Instance;
        ManagerStore managerStore = main.ManagerStore;
        podManager = managerStore.Get<PodManager>();
        spawnManager = managerStore.Get<SpawnManager>();

        StateManager stateManager = managerStore.Get<StateManager>();
        stateManager.AddListener(this);

        return true;
    }

    public void OnStateChanged(GameState previous, GameState current)
    {
        switch (current)
        {
            case GameState.Menu:
            case GameState.DeathScreen:

                CleanCurrentWave();
                SetWave(0);
                alive = false;

                break;
            case GameState.Playing:

                CleanCurrentWave();
                SetWave(0);
                alive = true;

                break;
        }
    }
}
