using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBob : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float bobRate = 1.5f;
    [SerializeField]
    private float bobHeight = 0.5f;

    private float bobValue;

    private void Update()
    {
        bobValue += Time.deltaTime;

        float bob = (Mathf.Sin(bobValue * bobRate) * bobHeight) + (bobHeight + 0.5f);
        Vector3 lPos = target.localPosition;
        target.localPosition = new Vector3(lPos.x, bob, lPos.z);
    }
}
