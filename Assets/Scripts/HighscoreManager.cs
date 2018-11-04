using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public interface IHighscoreListener
{
    void OnHighscoresChanged(List<HighscoreEntry> entries);
}

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
    private List<IHighscoreListener> listeners;

    public bool AddListener(IHighscoreListener listener)
    {
        if (!listeners.Contains(listener))
        {
            listeners.Add(listener);
            return true;
        }
        return false;
    }

    public bool RemoveListener(IHighscoreListener listener)
    {
        return listeners.Remove(listener);
    }

    private void Awake()
    {
        listeners = new List<IHighscoreListener>();

        InitScores();
        FetchScores();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            PlayerPrefs.DeleteAll();
            foreach(HighscoreEntry entry in entries)
            {
                entry.Score = 0;
            }
        }
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

    public void InsertScore(int score, bool autoSave)
    {
        // Add an entry with the given score
        HighscoreEntry entry = new HighscoreEntry();
        entry.Score = score;
        entries.Add(entry);

        // Order the entries and remove the lowest scoring entry
        entries = entries.OrderBy(v => v.Score).ToList();
        entries.RemoveAt(0);
        entries.Reverse();


        Debug.Log("UPDATING HIGHSCORES");
        // Notify other listeners that the scores have changed
        foreach (IHighscoreListener listener in listeners)
        {
            listener.OnHighscoresChanged(entries);
        }

        // Should the scores be saved?
        if (autoSave)
        {
            SaveScores();
        }
    }

    private void SaveScores()
    {
        for(int i = 0; i < scoreEntries; i++)
        {
            HighscoreEntry entry = entries[i];
            PlayerPrefs.SetInt("score_" + i.ToString(), entry.Score);
        }
    }

    private void FetchScores()
    {
        for (int i = 0; i < scoreEntries; i++)
        {
            int result = PlayerPrefs.GetInt("score_" + i.ToString(), -1);
            if (result != -1)
            {
                InsertScore(result, false);
            }
        }
    }

    public void OnStateChanged(GameState previous, GameState current)
    {
    }
}