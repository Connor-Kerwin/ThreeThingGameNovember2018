using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SetScore : MonoBehaviour {
    public Text scoreText;

    private int currentScore = 0;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Main main = Main.Instance;
        ManagerStore managerstore = main.ManagerStore;
        ScoreManager playermanager = managerstore.Get<ScoreManager>();
        currentScore = playermanager.Score;


        scoreText.text = currentScore.ToString();

    }
}
