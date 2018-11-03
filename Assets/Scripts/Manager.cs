using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public virtual bool Initialize()
    {
        return true;
    }

    public virtual bool Link()
    {
        return true;
    }
}
