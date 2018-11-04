using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualHighscore : MonoBehaviour
{
    [SerializeField]
    private Text text;

    public void SetScore(string score)
    {
        text.text = score;
    }
}
