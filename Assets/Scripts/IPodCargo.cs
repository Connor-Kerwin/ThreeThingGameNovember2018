using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPodCargo
{
    void OnCargoAttached(Pod pod);
    void OnCargoDetached(Pod pod);
}
