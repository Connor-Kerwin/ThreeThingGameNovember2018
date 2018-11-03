using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracer : MonoBehaviour
{
    private TracerPool pool;
    private Vector3 start;
    private Vector3 end;
    private float speed;
    private float dist;
    private float length;

    private bool alive;

    public void Link(TracerPool pool)
    {
        this.pool = pool;
    }

    public void Fire(Vector3 start, Vector3 end, float speed)
    {
        this.start = start;
        this.end = end;
        this.speed = speed;

        alive = true;
        dist = 0.0f;
        length = Vector3.Distance(start, end);
    }

    private void Update()
    {
        if (alive) // Is the tracer alive?
        {
            dist += Time.deltaTime * speed;
            if (dist >= length) // Enough time passed to die?
            {
                Kill();
            }

            // Move to position
            float f = dist / length;
            Vector3 pos = Vector3.Lerp(start, end, f);
            transform.position = pos;
        }
    }

    private void Kill()
    {
        alive = false;
        pool.KillTracer(this);
    }
}
