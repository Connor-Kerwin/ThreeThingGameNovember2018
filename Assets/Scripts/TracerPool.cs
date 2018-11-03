using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracerPool
{
    private Tracer prefab;
    private Pool<Tracer> tracers;

    public TracerPool(Tracer prefab)
    {
        this.prefab = prefab;
        tracers = new Pool<Tracer>(Create);
    }

    public void SpawnTracer(Vector3 start, Vector3 end, float speed)
    {
        Tracer tracer = tracers.Get();
        tracer.Fire(start, end, speed);
    }

    public void KillTracer(Tracer tracer)
    {
        tracers.Store(tracer);
    }

    private Tracer Create()
    {
        Tracer tracer = GameObject.Instantiate<Tracer>(prefab);
        tracer.Link(this);
        return tracer;
    }
}
