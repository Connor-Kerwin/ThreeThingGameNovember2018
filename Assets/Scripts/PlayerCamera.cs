using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour, IStateMachineListener<GameState>
{
    [SerializeField]
    private float turnRate = 90.0f;
    [SerializeField]
    private float yLimit = 70.0f;

    private float rX, rY;
    private bool alive;

    public void OnStateChanged(GameState previous, GameState current)
    {
        switch (current)
        {
            case GameState.Menu:
            case GameState.DeathScreen:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                alive = false;
                break;
            case GameState.Playing:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                alive = true;
                break;
        }
    }

    // Use this for initialization
    void Start ()
    {
        Main main = Main.Instance;
        ManagerStore managerStore = main.ManagerStore;
        StateManager stateManager = managerStore.Get<StateManager>();
        stateManager.AddListener(this);
        alive = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (alive) // Only move camera if alive
        {
            // Fetch axes
            float x = Input.GetAxis("Mouse X") * Time.deltaTime * turnRate;
            float y = Input.GetAxis("Mouse Y") * Time.deltaTime * turnRate;

            // CLAMP
            rX += x;
            rY = Mathf.Clamp(rY + y, -yLimit, yLimit);

            // Apply rotation
            transform.rotation = Quaternion.Euler(-rY, rX, 0);
        }
    }
}
