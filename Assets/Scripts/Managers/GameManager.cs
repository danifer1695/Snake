using System;
using System.Runtime;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    //Public variables
    //public static event Action GameClock;

    [Header("Map")]
    public MapManager mapManager;
    public float foodSpawnFrequence = 5.0f; //in seconds

    [Header("Settings")]
    public GameClock gameClock;
    public InputActionMap controlActions;
    public bool soundOn = true;

    //Private variables
    private float foodTimer;
    private bool gameActive = true;
    private bool gamePaused = false;

    private InputAction pauseAction;

    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        PollInput();
    }

    private void OnDestroy()
    {
        GameClock.clockTick -= FoodSpawnTimer;
    }

    //=======================================================================
    //Init
    //=======================================================================
    private void Init()
    {
        //Set current scene as active
        SceneManager.SetActiveScene(gameObject.scene);

        //Initialize actions
        controlActions.Enable();
        pauseAction = controlActions.FindAction("Pause");
        GameClock.clockTick += FoodSpawnTimer;

        //Initialize managers
        mapManager.Initialize();

        Debug.Log("Game Started");
    }
    //=======================================================================
    //Input
    //=======================================================================
    private void PollInput()
    {
        //Actions
        if (pauseAction.triggered) Pause();
    }

    //=======================================================================
    //Timers
    //=======================================================================
    private void FoodSpawnTimer()
    {
        foodTimer += 1.0f;

        if(foodTimer >= foodSpawnFrequence)
        {
            mapManager.SpawnFood();
            foodTimer = 0.0f;
        }
    }
  
    //=======================================================================
    //Game State
    //=======================================================================
    public void GameOver()
    {
        //set game state
        gameActive = false;
        controlActions.Disable();

        //show 'GAME OVER' text
        UIManager.Instance.ShowGameOverMenu();
    }
    //-------------------------------------------------
    public void Pause()
    {
        if(!gamePaused)
        {
            gameActive = false;
            gamePaused = true;
            UIManager.Instance.ShowPauseMenu();
        }
        else
        {
            gameActive = true;
            gamePaused = false;
            UIManager.Instance.HidePauseMenu();
        }
    }
    //-------------------------------------------------
    public void ToggleSound()
    {
        soundOn = !soundOn;
    }
}

