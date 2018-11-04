using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class HighscoreEntry
{
    [SerializeField]
    private int score;

    public int Score { get { return score; }  set { score = value; } }
}

public class HighscoreManager : Manager, IStateMachineListener<GameState>
{
    [SerializeField]
    private List<HighscoreEntry> entries;

    private int scoreEntries = 5;

    private void Awake()
    {
        InitScores();
        FetchScores();
    }

    public override bool Link()
    {
        Main main = Main.Instance;
        ManagerStore managerStore = main.ManagerStore;
        StateManager stateManager = managerStore.Get<StateManager>();
        stateManager.AddListener(this);

        return base.Link();
    }

    private void InitScores()
    {
        for (int i = 0; i < scoreEntries; i++)
        {
            entries.Add(new HighscoreEntry());
        }
    }

    public void InsertScore(int score)
    {
        // Add an entry with the given score
        HighscoreEntry entry = new HighscoreEntry();
        entry.Score = score;
        entries.Add(entry);

        // Order the entries and remove the lowest scoring entry
        entries.OrderBy(v => v.Score);
        entries.RemoveAt(0);
    }

    private void FetchScores()
    {
        for (int i = 0; i < scoreEntries; i++)
        {
            int result = PlayerPrefs.GetInt("score_" + i.ToString(), -1);
            if (result != -1)
            {
                InsertScore(result);
            }
        }
    }

    public void OnStateChanged(GameState previous, GameState current)
    {
    }
}