using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Manager
{
    [SerializeField]
    private List<Spawnable> prefabs;

    public override bool Initialize()
    {
        return false;
    }
}
