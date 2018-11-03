using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodManager : Manager
{
    [SerializeField]
    private string podID;

    private List<Pod> pods;
    private SpawnManager spawnManager;

    public override bool Link()
    {
        // Fetch the spawn manager
        Main main = Main.Instance;
        ManagerStore managerStore = main.ManagerStore;
        spawnManager = managerStore.Get<SpawnManager>();

        return true;
    }

    private void Awake()
    {
        pods = new List<Pod>();
    }

    public void CleanPod(Pod pod)
    {
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
}
