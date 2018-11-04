using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFlash : MonoBehaviour
{
    [SerializeField]
    private float flashLoss;
    [SerializeField]
    private float flashValue = 0.25f;
    [SerializeField]
    private Renderer target;

    private float flash;


    public void Flash()
    {
        flash = flashValue;
    }

    private void Update()
    {
        flash -= Time.deltaTime * flashLoss;
        flash = Mathf.Clamp(flash, 0.0f, 1000.0f);
        Material m = target.material;
        Color c = m.color;
        c.a = flash;
        m.color = c;
    }
}
