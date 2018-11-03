using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private bool initialized;
    private static Main instance;

    public bool Initialized { get { return initialized; } }
    public static Main Instance { get { return instance; } }

    private void Awake()
    {
        instance = this;

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
        return false;
    }

    private bool Initialize()
    {
        return false;
    }

}
