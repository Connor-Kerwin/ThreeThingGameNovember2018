using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
	// Update is called once per frame
	void Update ()
    {
        Camera cam = Camera.main;
        Vector3 targetPos = cam.transform.position;
        targetPos.y = transform.position.y;

        transform.LookAt(targetPos, Vector3.up);
	}
}
