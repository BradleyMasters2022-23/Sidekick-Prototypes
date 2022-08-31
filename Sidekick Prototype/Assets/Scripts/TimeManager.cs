using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimeManager : MonoBehaviour
{
    public static float worldTime;

    public float normalTime;
    public float slowedTime;

    PlayerControls controller;
    InputAction slow;

    bool slowing;
    
    private void Start()
    {
        controller = new PlayerControls();
        slowing = false;
        StartTime();

        slow = controller.Player.SlowTime;
        slow.performed += ToggleSlow;
        slow.Enable();
    }

    private void OnDisable()
    {
        slow.Disable();
    }

    public void SlowTime()
    {
        Debug.Log("Freezing Time");
        worldTime = slowedTime;
    }

    public void StartTime()
    {
        Debug.Log("Starting Time");
        worldTime = normalTime;
    }

    private void ToggleSlow(InputAction.CallbackContext ctx)
    {
        slowing = !slowing;

        if(slowing)
        {
            StartTime();
        }
        else
        {
            SlowTime();
        }
    }

}
