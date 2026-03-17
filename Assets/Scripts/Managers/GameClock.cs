using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class GameClock : MonoBehaviour
{
    //Public variables
    public static event Action clockTick;

    private float timer;

    [Header("Clock Frequency")]
    public float speedUpFrequency = 10.0f; //in seconds
    public float speedUpFactor = 0.95f;

    private float frequency = 1.0f;
    private float speedUpCounter = 0.0f;

    private void Start()
    {
        clockTick += Accelerate;
    }

    private void Update()
    {
        Timer();
    }

    private void OnDestroy()
    {
        clockTick -= Accelerate;
    }

    //=======================================================================
    //Timers
    //=======================================================================
    private void Timer()
    {
        //in Seconds
        timer += Time.deltaTime;

        //if 'moveFrequence' seconds have passed
        if (timer >= frequency)
        {
            //Invoke event
            clockTick?.Invoke();
            timer = 0.0f;
        }
    }

    //=======================================================================
    //SetFrequency
    //=======================================================================
    public void Accelerate()
    {
        speedUpCounter++;

        if(speedUpCounter >= speedUpFrequency)
        {
            Debug.Log("Game Clock accelerated. New frequency: " + frequency);
            frequency *= speedUpFactor;
            speedUpCounter = 0.0f;
        }
    }
}
