using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class Snake : MonoBehaviour
{
    //Public variables
    //========================================
    public Vector3 lastPosition;
    public List<GameObject> tail;
    public GameObject eye;
    public GameObject tailSegmentPrefab;

    [Header("Movement")]
    public float upperLowerBoundary = 7.0f;
    public float leftRightBoundary = 11.0f;

    [Header("Actions")]
    public InputAction moveAction;

    [Header("State")]
    public bool invincible = false;

    [Header("Audio")]
    [SerializeField]private AudioSource audioSource;
    [SerializeField]private AudioClip foodAudio;
    [SerializeField]private AudioClip moveAudio;
    [SerializeField]private AudioClip deathAudio;

    //Private variables
    //========================================
    private Vector3 nextDirection = new Vector3(0.0f, 0.0f, 1.0f);  //Direction the snake will be moving towards
    private Vector3 currentDirection = Vector3.zero;    //Direction the snake last moved towards
    private GameManager gm; //Reference to Game Manager
    

    void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        //disable actions
        moveAction.Disable();

        //unsubscribe from events
        GameManager.GameClock -= Move;
    }

    //Collisions
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision detected: " + other.name);
        //If we collide with a 'Food' tagged object, instantiate a tail segment into the tail list
        if (other.CompareTag("Food")) FoodCollision(other);
        if (other.CompareTag("Tail") || 
            other.CompareTag("Wall")) TailCollision(other);
    }

    //Update
    void Update()
    {
        UpdateMoveDir();
    }
    //=======================================================================
    //Init
    //=======================================================================

    private void Init()
    {
        //Initialize variables
        gm = FindAnyObjectByType<GameManager>();    //Get Game Manager
        lastPosition = transform.position;

        //Starting eye position
        MoveEye(Vector3.forward);

        //enable actions
        moveAction.Enable();

        //subscribe to events
        GameManager.GameClock += Move;
    }
    //=======================================================================
    //Movement
    //=======================================================================

    //update the direction the object will be moving in next time GM calls Move()
    private void UpdateMoveDir()
    {
        //if there is no input, return
        if (moveAction == null) return;

        //Action returns a Vector3 value
        Vector3 input = moveAction.ReadValue<Vector3>();

        //if input is zero, return
        //we also dont want the snake to be able to do a 180, so we return
        //if the input equals the opposite of the current direction
        if (input == Vector3.zero ||
            input == -currentDirection)
            return;

        //Move eye so it follows the desired direction
        MoveEye(input);

        float absX = Mathf.Abs(input.x);
        float absZ = Mathf.Abs(input.z);

        if (absX >= absZ && absX >= absZ)
            nextDirection = input.x > 0 ? Vector3.right : Vector3.left;
        else
            nextDirection = input.z > 0 ? Vector3.forward : Vector3.back;
    }
    
    //Move() is called by GameClock event
    private void Move()
    {
        //Play sound
        if(gm.soundOn) audioSource.PlayOneShot(moveAudio);

        //Reset invincibility
        invincible = false;

        //Update 'lastPosition' before we update current position
        lastPosition = transform.position;

        //Update position
        transform.position = BoundaryCheck(transform.position + nextDirection);

        //Update current direction
        currentDirection = nextDirection;

        //update tail
        UpdateTail();
    }

    private Vector3 BoundaryCheck(Vector3 expectedPos)
    {
        Vector3 newPos = expectedPos;

        //Check if expected Position surpasses boundaries
        //If boundaries are surpassed, teleport to opposite
        //side of the screen
        if (newPos.z >= upperLowerBoundary)         newPos.z = -upperLowerBoundary + 1.0f;
        else if (newPos.z <= -upperLowerBoundary)   newPos.z = upperLowerBoundary - 1.0f;
        else if (newPos.x >= leftRightBoundary)     newPos.x = -leftRightBoundary + 1.0f;
        else if (newPos.x <= -leftRightBoundary)    newPos.x = leftRightBoundary - 1.0f;

        //Return new position
        return newPos;
    }

    private void MoveEye(Vector3 direction)
    { 
        //rotate in world coordinates (transform.rotation always rotates in world coords)
        //Argument 1 = direction vector
        //Argument 2 = rotation axis
        eye.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }
    //=======================================================================
    //Collisions
    //=======================================================================

    private void FoodCollision(Collider other)
    {
        //Play sound
        if(gm.soundOn) audioSource.PlayOneShot(foodAudio);

        //Player is invincible while eating
        invincible = true;

        //if there is no tail, use head's position as spawn position
        //if there is a tail, use last tail segment's position as spawn position
        Vector3 spawnPos = (tail.Count == 0) ? transform.position : tail[tail.Count - 1].transform.position;
        tail.Add(Instantiate(tailSegmentPrefab, spawnPos, Quaternion.identity));

        Destroy(other.gameObject);
    }

    private void TailCollision(Collider other)
    {
        //End game
        if(!invincible) 
        {
            //Play sound
            if (gm.soundOn) audioSource.PlayOneShot(deathAudio);

            gm.GameOver(); 
        }
    }
    //=======================================================================
    //UpdateTail
    //=======================================================================

    private void UpdateTail()
    {
        int tailSize = tail.Count;

        //if there is no tail, return
        if (tailSize == 0) return;


        //Set lastPosition to the first tail 
        Vector3 lastPos = lastPosition;
        lastPos = tail[0].transform.position;
        tail[0].transform.position = lastPosition;

        for(int i = 1; i < tailSize; i++)
        {
            //Keep tail[i]'s position in a temp value
            Vector3 temp = tail[i].transform.position;

            //set tail[i]'s position to lastPos
            tail[i].transform.position = lastPos;

            //set lastPos to the value stored in 'temp'
            lastPos = temp;
        }

    }
}
