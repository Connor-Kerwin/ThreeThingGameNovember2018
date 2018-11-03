using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMainListener
{
    /// <summary>
    /// Called when everything has finished initialization within the management system.
    /// </summary>
    /// <param name="main"></param>
    void OnFinishedInitialization(Main main);
    /// <summary>
    /// Called when everything has finished LINK step. (Kind of like a pre-finished but not fully finished)
    /// Useful for linking non managers to managers.
    /// </summary>
    /// <param name="main"></param>
    void OnFinishedLink(Main main);
}

public class Main : MonoBehaviour
{
    private bool initialized;
    private static Main instance;
    private ManagerStore managerStore;
    private List<IMainListener> listeners;

    public bool Initialized { get { return initialized; } }
    public ManagerStore ManagerStore { get { return managerStore; } }
    public static Main Instance { get { return instance; } }

    public bool AddListener(IMainListener listener)
    {
        if(!listeners.Contains(listener))
        {
            listeners.Add(listener);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool RemoveListener(IMainListener listener)
    {
        return listeners.Remove(listener);
    }

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
        else
        {
            foreach(IMainListener listener in listeners)
            {
                listener.OnFinishedInitialization(this);
            }
        }
    }

    private bool PreInitialize()
    {
        instance = this;
        managerStore = new ManagerStore();
        listeners = new List<IMainListener>();

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

        // FINISHED LINK
        foreach (IMainListener listener in listeners)
        {
            listener.OnFinishedLink(this);
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
