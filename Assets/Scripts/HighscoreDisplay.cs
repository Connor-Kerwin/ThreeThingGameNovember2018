using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreDisplay : MonoBehaviour, IHighscoreListener
{
    [SerializeField]
    private List<VisualHighscore> highscores;

    public void OnHighscoresChanged(List<HighscoreEntry> entries)
    {
        // Get a valid index length
        int eC = entries.Count;
        int hC = highscores.Count;
        int fC = hC >= eC ? eC : hC;

        // Iterate displays
        for(int i = 0; i < fC; i++)
        {
            HighscoreEntry entry = entries[i];
            VisualHighscore display = highscores[i];
            display.SetScore(entry.Score.ToString());
            display.SetName(entry.Name);
        }
    }

    private void Start()
    {
        Main main = Main.Instance;
        ManagerStore managerStore = main.ManagerStore;
        HighscoreManager highscoreManager = managerStore.Get<HighscoreManager>();
        highscoreManager.AddListener(this);
    }
}
