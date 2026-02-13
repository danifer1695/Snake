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
    public static event Action GameClock;

    [Header("Food")]
    public GameObject foodPrefab;
    public float foodSpawnFrequence = 5.0f; //in seconds

    [Header("Movement")]
    public float moveFrequence = 1.0f; //in seconds

    [Header("Movement Speed Up")]
    public float speedUpFrequence = 10.0f; //in seconds
    public float speedUpFactor = 0.95f;

    [Header("Settings")]
    public InputActionMap controlActions;
    public bool soundOn = true;

    [Header("UI")]
    public GameObject gameOverText;
    public GameObject pauseText;

    [Header("Scenes")]
    public int gameSceneIndex = 0;
    public int titleSceneIndex = 1;

    //Private variables
    private float moveTimer;
    private float foodTimer;
    private float speedUpTimer;
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

        if (gameActive)
        {
            MoveTimer(moveFrequence);
            FoodSpawnTimer(foodSpawnFrequence);
            SpeedUpTimer(speedUpFrequence);
        }
    }
    //=======================================================================
    //Init
    //=======================================================================
    private void Init()
    {
        //Initialize actions
        controlActions.Enable();
        pauseAction = controlActions.FindAction("Pause");

        //check states
        gameOverText.SetActive(false);
        pauseText.SetActive(false);

        //Spawn one food item
        SpawnFood();

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
    private void MoveTimer(float frequence)
    {
        //in Seconds
        moveTimer += Time.deltaTime;

        //if 'moveFrequence' seconds have passed
        if (moveTimer >= frequence)
        {
            //Invoke event
            GameClock?.Invoke();
            //move timer back by one second
            moveTimer = 0.0f;
        }
    }
    //-------------------------------------------------
    private void FoodSpawnTimer(float frequence)
    {
        //update timer
        foodTimer += Time.deltaTime;

        //if if 'frequence' seconds have passed
        if (foodTimer >= frequence)
        {
            SpawnFood();
            
            //move timer back by 'frequence' seconds
            foodTimer = 0.0f;
        }
    }
    //-------------------------------------------------
    private void SpeedUpTimer(float frequence)
    {
        //update timer
        speedUpTimer += Time.deltaTime;

        //if if 'frequence' seconds have passed
        if (speedUpTimer >= frequence)
        {
            //speed up movement frequence
            moveFrequence *= speedUpFactor;
            Debug.Log("Movement frequence changed to: " + moveFrequence);

            //move timer back by 'frequence' seconds
            speedUpTimer = 0.0f;
        }
    }
    //=======================================================================
    //GameObject Instantiation
    //=======================================================================
    
    private void SpawnFood()
    {
        //Calculate spawn coordinates
        float coorX = UnityEngine.Random.Range(-7.5f, 7.5f);
        float coorZ = UnityEngine.Random.Range(-5.0f, 5.0f);
        Vector3 spawnCoords = new Vector3(((int)coorX), 0.0f, ((int)coorZ));

        //Spawn food item
        Instantiate(foodPrefab, spawnCoords, Quaternion.identity);
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
        gameOverText.SetActive(true);
    }
    //-------------------------------------------------
    public void Pause()
    {
        if(!gamePaused)
        {
            gameActive = false;
            gamePaused = true;
            pauseText.SetActive(true);
        }
        else
        {
            gameActive = true;
            gamePaused = false;
            pauseText.SetActive(false);
        }
    }
    //-------------------------------------------------
    public void ToggleSound()
    {
        soundOn = !soundOn;
    }
}

