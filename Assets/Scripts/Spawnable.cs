using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnable : MonoBehaviour {

    [SerializeField]
    private string id;

    public string ID { get { return id; } }
}
