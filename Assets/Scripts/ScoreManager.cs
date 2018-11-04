using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : Manager, IStateMachineListener<GameState>
{
    [SerializeField]
    private int score;

    private HighscoreManager highscoreManager;

    public int Score { get { return score; } }

    public override bool Link()
    {
        Main main = Main.Instance;
        ManagerStore managerStore = main.ManagerStore;
        StateManager stateManager = managerStore.Get<StateManager>();
        stateManager.AddListener(this);

        highscoreManager = managerStore.Get<HighscoreManager>();

        return base.Link();
    }

    public void SetScore(int score)
    {
        if(ScoreValid(score))
        {
            this.score = score;
        }
    }

    public void AddScore(int score)
    {
        if(ScoreValid(score))
        {
            this.score += score;
        }
    }

    private bool ScoreValid(int value)
    {
        return value >= 0;
    }

    public void OnStateChanged(GameState previous, GameState current)
    {
        switch (current)
        {
            case GameState.Menu:
            case GameState.Playing:

                // Change score to zero
                SetScore(0);

                break;
            case GameState.DeathScreen:
                highscoreManager.InsertScore(score, true);
                break;
        }
    }
}
