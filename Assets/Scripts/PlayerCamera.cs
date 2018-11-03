using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    private float turnRate = 90.0f;
    [SerializeField]
    private float yLimit = 70.0f;

    private float rX, rY;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Fetch axes
        float x = Input.GetAxis("Mouse X") * Time.deltaTime * turnRate;
        float y = Input.GetAxis("Mouse Y") * Time.deltaTime * turnRate;

        // CLAMP
        rX += x;
        rY = Mathf.Clamp(rY + y, -yLimit, yLimit);

        // Apply rotation
        transform.rotation = Quaternion.Euler(-rY, rX, 0);
    }
}
