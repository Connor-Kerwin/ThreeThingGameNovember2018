using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pod : MonoBehaviour
{
    [SerializeField]
    private LayerMask mask;
    [SerializeField]
    private float fallTime = 2.0f;

    [SerializeField]
    private bool falling;
    [SerializeField]
    private float targetY;
    [SerializeField]
    private float startY;
    [SerializeField]
    private float time;

    private Transform target;

    public void SetCargo(Transform target)
    {
        this.target = target;
        target.parent = transform;
        target.SendMessage("OnCargoAttached", this, SendMessageOptions.DontRequireReceiver);
    }

    private void Land()
    {
        target.parent = null;
        target.SendMessage("OnCargoDetached", this, SendMessageOptions.DontRequireReceiver);
        target = null;
    }

    public void Launch(Vector3 position)
    {
        startY = position.y;
        falling = true;

        Ray ray = new Ray(position, Vector3.down);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 1000.0f, mask))
        {
            targetY = hit.point.y;
        }
        else // Assume zero is the point if the pod isnt on the floor for some reason
        {
            targetY = 0;
        }
    }

    private void Update()
    {
        if(falling)
        {
            time += Time.deltaTime;

            if(time >= fallTime)
            {
                falling = false;
                Vector3 pos = transform.position;
                transform.position = new Vector3(pos.x, targetY, pos.z);
            }
            else
            {
                float f = time / fallTime;
                Vector3 pos = transform.position;
                transform.position = new Vector3(pos.x, Mathf.Lerp(startY, targetY, f), pos.z);
            }
        }
    }

}
