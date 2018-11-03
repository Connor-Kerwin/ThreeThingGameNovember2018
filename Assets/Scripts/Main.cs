using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private bool initialized;
    private static Main instance;
    private ManagerStore managerStore;

    public bool Initialized { get { return initialized; } }
    public static Main Instance { get { return instance; } }

    private void Awake()
    {
        if(!PreInitialize())
        {
            Debug.LogError("Pre-initialize failed");
        }
    }

    private void Start()
    {
        if(!Initialize())
        {
            Debug.LogError("Initialize failed");
        }
    }

    private bool PreInitialize()
    {
        instance = this;
        managerStore = new ManagerStore();

        return true;
    }

    private bool Initialize()
    {
        FetchManagers();

        if(!InitializeManagers())
        {
            return false;
        }

        if(!LinkManagers())
        {
            return false;
        }

        initialized = true;
        return true;
    }

    private bool InitializeManagers()
    {
        bool flag = true;

        foreach (Manager manager in managerStore)
        {
            if (!manager.Initialize())
            {
                Debug.LogError(manager.GetType() + " failed to initialize");
                flag = false;
            }
        }

        return flag;
    }

    private bool LinkManagers()
    {
        bool flag = true;

        foreach (Manager manager in managerStore)
        {
            if (!manager.Link())
            {
                Debug.LogError("Manager " + manager.GetType() + " failed to link");
                flag = false;
            }
        }

        return flag;
    }

    private void FetchManagers()
    {
        Manager[] managers = GetComponents<Manager>();
        foreach(Manager manager in managers)
        {
            managerStore.Add(manager);
        }
    }

}
