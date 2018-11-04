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
    [SerializeField]
    private string name;

    public int Score { get { return score; }  set { score = value; } }
    public string Name { get { return name; } set { name = value; } }

    public HighscoreEntry(int score, string name)
    {
        this.score = score;
        this.name = name;
    }
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

        // Initially populate scores
        for(int i = 0; i < scoreEntries; i++)
        {
            entries.Add(new HighscoreEntry(0, ""));
        }

        // Did scores fail to fetch?
        if(!FetchScores())
        {
            InitScores();
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            InitScores();
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
            HighscoreEntry entry = entries[i];
            entry.Score = GetScoreForEntry(i);
            entry.Name = GetRandomName();
        }
    }

    private string GetRandomName()
    {
        // Inefficient but rarely called so it's fine.
        string[] names = new string[]
        {
            "NAME_01",
            "NAME_02",
            "NAME_03",
            "NAME_04",
            "NAME_05",
            "NAME_06",
            "NAME_07",
            "NAME_08",
        };

        int r = Random.Range(0, names.Length);
        return names[r];
    }

    private int GetScoreForEntry(int index)
    {
        return (index + 1) * 100;
    }

    public void InsertScore(int score, string name, bool autoSave)
    {
        // Add an entry with the given score
        HighscoreEntry entry = new HighscoreEntry(score, name);
        entries.Add(entry);

        // Order the entries and remove the lowest scoring entry
        entries = entries.OrderBy(v => v.Score).ToList();
        entries.RemoveAt(0);
        entries.Reverse();

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
            PlayerPrefs.SetString("name_" + i.ToString(), entry.Name);
        }
    }

    private bool FetchScores()
    {
        bool flag = true;

        for (int i = 0; i < scoreEntries; i++)
        {
            int result = PlayerPrefs.GetInt("score_" + i.ToString(), -1);
            string name = PlayerPrefs.GetString("name_" + i.ToString(), "INVALID_NAME_VALUE");
            if (result != -1 && name != "INVALID_NAME_VALUE") // Is name and score valid?
            {
                InsertScore(result, name, false);
            }
            else // Score missing
            {
                flag = false;
            }
        }

        return flag;
    }

    public void OnStateChanged(GameState previous, GameState current)
    {
    }
}