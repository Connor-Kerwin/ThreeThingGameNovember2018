using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanicFlip : MonoBehaviour
{
    [SerializeField]
    private float flipRate = 0.1f;
    private float time;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        time += Time.deltaTime;
        if(time >= flipRate)
        {
            time = 0.0f;

            Vector3 ls = transform.localScale;
            ls.x *= -1.0f;
            transform.localScale = ls;
        }
	}
}
