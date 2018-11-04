using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimate : MonoBehaviour
{
    [SerializeField]
    private float rate = 1.0f;
    [SerializeField]
    private float magnitude = 1.0f;

    [SerializeField]
    private float moveRate = 1.0f;
    [SerializeField]
    private float moveMagnitude = 1.0f;
    
    [SerializeField]
    private float time = 0.0f;
    [SerializeField]
    private float moveTime = 0.0f;
    private Vector3 cachedPos;

	// Use this for initialization
	void Start ()
    {
        cachedPos = transform.position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        time += Time.deltaTime;
        float mod = Mathf.Sin(time * rate) * magnitude;
        Vector3 rot = transform.rotation.eulerAngles;
        rot.z = mod;

        transform.rotation = Quaternion.Euler(rot);

        moveTime += Time.deltaTime;
        float moveMod = Mathf.Sin(moveTime * moveRate) * moveMagnitude;
        Vector3 pos = cachedPos;
        pos.y += moveMod;
        transform.position = pos;
	}
}
