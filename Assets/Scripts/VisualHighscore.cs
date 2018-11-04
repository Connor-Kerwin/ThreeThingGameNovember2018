using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualHighscore : MonoBehaviour
{
    [SerializeField]
    private Text text;
    [SerializeField]
    private Text nameText;

    public void SetScore(string score)
    {
        text.text = score;
    }

    public void SetName(string name)
    {
        nameText.text = name;
    }
}
