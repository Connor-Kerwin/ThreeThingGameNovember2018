using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimatedImage : MonoBehaviour {

    [SerializeField]
    private Sprite[] sprites;
    [SerializeField]
    private Image targetImage;
    [SerializeField]
    private float rate = 0.1f;

    float time;
    int index;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        time += Time.deltaTime;
        if(time >= rate)
        {
            time = 0.0f;
            Step();
        }
	}

    private void Step()
    {
        index++;
        if (index >= sprites.Length)
        {
            index = 0;
        }

        targetImage.sprite = sprites[index];
    }
}
