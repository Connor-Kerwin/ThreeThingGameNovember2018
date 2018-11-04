using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanicFlip : MonoBehaviour
{
    [SerializeField]
    private float randomOffsetMin = 0.01f;
    [SerializeField]
    private float randomOffsetMax = 0.05f;
    [SerializeField]
    private float flipRate = 0.1f;

    private float time;
    private float actualFlipRate;

	// Use this for initialization
	void Start ()
    {
        actualFlipRate = flipRate - Random.Range(randomOffsetMin, randomOffsetMax);
	}
	
	// Update is called once per frame
	void Update ()
    {
        time += Time.deltaTime;
        if(time >= actualFlipRate)
        {
            time = 0.0f;

            Vector3 ls = transform.localScale;
            ls.x *= -1.0f;
            transform.localScale = ls;
        }
	}
}
