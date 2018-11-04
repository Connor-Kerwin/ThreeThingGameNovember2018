using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameSelect : MonoBehaviour, IStateMachineListener<GameState>
{
    [SerializeField]
    private InputField field;
    private ScoreManager scoreManager;

    public void OnStateChanged(GameState previous, GameState current)
    {
        // Catch transition to game
        if(current == GameState.Playing)
        {
            string inName = field.text;

            // Fix empty name
            if (string.IsNullOrEmpty(inName))
            {
                inName = "Player";
            }

            scoreManager.Name = inName.ToUpper();
        }
    }

    // Use this for initialization
    void Start ()
    {
        Main main = Main.Instance;
        ManagerStore managerStore = main.ManagerStore;
        StateManager stateManager = managerStore.Get<StateManager>();
        stateManager.AddListener(this);
        scoreManager = managerStore.Get<ScoreManager>();
	}
}
