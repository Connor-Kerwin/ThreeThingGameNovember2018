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
    private ParticleSystem particles;

    [SerializeField]
    private bool falling = false;
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
        target.SendMessage("ToggleOff", SendMessageOptions.DontRequireReceiver);
    }

    private void Land()
    {
        // Set the target position to self
        target.transform.position = transform.position;

        // Kill self
        Kill();
    }

    public void Kill()
    {
        target.SendMessage("OnCargoDetached", this, SendMessageOptions.DontRequireReceiver);
        target.SendMessage("ToggleOn", SendMessageOptions.DontRequireReceiver);

        time = 0.0f;
        falling = false;
        target.parent = null;
        target = null;

        particles.Stop();

        // Fetch the spawn manager
        Main main = Main.Instance;
        ManagerStore managerStore = main.ManagerStore;
        PodManager podManager = managerStore.Get<PodManager>();
        podManager.CleanPod(this);
    }

    public void Launch(Vector3 position)
    {
        particles.Play(); 

        startY = position.y;
        falling = true;
        time = 0.0f;

        Ray ray = new Ray(position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000.0f, mask))
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
        if (falling) // Finished falling
        {
            time += Time.deltaTime;

            if (time >= fallTime) // Has it landed?
            {
                Vector3 pos = transform.position;
                transform.position = new Vector3(pos.x, targetY, pos.z);

                falling = false;
                Land();
            }
            else // Falling still
            {
                float f = time / fallTime;
                Vector3 pos = transform.position;
                transform.position = new Vector3(pos.x, Mathf.Lerp(startY, targetY, f), pos.z);
            }
        }
    }
}