using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialManager : Manager
{
    [SerializeField]
    private Transform sun;
    [SerializeField]
    private Transform moon;

    [SerializeField]
    private float celestialRate = 5.0f;
    [SerializeField]
    private float celestialHeight = 10.0f;
    [SerializeField]
    private float celestialRadius = 25.0f;

    private float celestialAngle;

    private void Update()
    {
        celestialAngle += Time.deltaTime * celestialRate;

        float a = celestialAngle;
        float b = a + Mathf.PI;

        Vector3 pA = ParametricCircle(a, celestialHeight, celestialRadius);
        Vector3 pB = ParametricCircle(b, celestialHeight, celestialRadius);

        sun.transform.position = pA;
        moon.transform.position = pB;
    }

    Vector3 ParametricCircle(float ang, float height, float radius)
    {
        float x = radius * Mathf.Sin(ang);
        float z = radius * Mathf.Cos(ang);

        return new Vector3(x, height, z);
    }
}
