using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodManager : Manager
{
    [SerializeField]
    private string podID;

    private List<Pod> pods;

    private void Awake()
    {
        pods = new List<Pod>();
    }

    private Pod GetPod()
    {
        // Fetch the spawn manager
        Main main = Main.Instance;
        ManagerStore managerStore = main.ManagerStore;
        SpawnManager spawnManager = managerStore.Get<SpawnManager>();

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
