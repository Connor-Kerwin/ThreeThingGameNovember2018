using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodManager : Manager, IStateMachineListener<GameState>
{
    [SerializeField]
    private string podID;

    private List<Pod> pods;
    private List<Pod> active;
    private SpawnManager spawnManager;

    public override bool Link()
    {
        // Fetch the spawn manager
        Main main = Main.Instance;
        ManagerStore managerStore = main.ManagerStore;
        spawnManager = managerStore.Get<SpawnManager>();
        StateManager stateManager = managerStore.Get<StateManager>();
        stateManager.AddListener(this);

        return true;
    }

    private void Awake()
    {
        pods = new List<Pod>();
        active = new List<Pod>();
    }

    public void CleanPod(Pod pod)
    {
        active.Remove(pod);

        Spawnable spawnable = pod.GetComponent<Spawnable>();
        if(spawnable != null)
        {
            spawnManager.Store(spawnable);
        }
    }

    private Pod GetPod()
    {
        Spawnable instance;
        if (spawnManager.Spawn(podID, out instance))
        {
            // ASSUMPTION THAT POD WILL BE ON THE GIVEN SPAWNABLE
            Pod pod = instance.GetComponent<Pod>();
            active.Add(pod);

            return pod;
        }
        else // Spawnable not found
        {
            return null;
        }
    }

    public Pod SpawnPod(Vector3 position, Transform cargo)
    {
        Pod pod = GetPod();
        pod.transform.position = position;
        pod.SetCargo(cargo);
        return pod;
    }

    public void OnStateChanged(GameState previous, GameState current)
    {
        switch (current)
        {
            case GameState.Menu:
            case GameState.DeathScreen:
                int count = active.Count;
                for(int i = 0; i < count; i++)
                {
                    Pod pod = active[0];
                    pod.Kill();
                    CleanPod(pod);
                }
                break;
        }
    }
}
