using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimeManager : MonoBehaviour
{
    public static float worldTime;

    public bool toggle;

    [HideInInspector] public float normalTime = 1;
    [Range(0, 1)]
    public float slowedTime;
    public float timeChangeSpeed;

    PlayerControls controller;
    InputAction slow;
    InputAction slowHold;

    bool slowing;
    
    private void Start()
    {
        controller = new PlayerControls();
        slowing = false;
        StartTime();

        if(toggle)
        {
            slow = controller.Player.SlowTime;
            slow.performed += ToggleSlow;
            slow.Enable();
        }
        else
        {
            slowHold = controller.Player.HoldSlow;
            slowHold.started += ToggleSlow;
            slowHold.canceled += ToggleSlow;
            slowHold.Enable();
        }
        
    }


    private void OnDisable()
    {
        if(toggle)
        {
            slow.Disable();
        }
        else
        {
            slowHold.Disable();
        }
    }

    private void FixedUpdate()
    {
        if (slowing && worldTime != slowedTime)
        {
            worldTime = Mathf.Lerp(worldTime, slowedTime, timeChangeSpeed);
            if (worldTime < 0.05)
                worldTime = 0;
        }
        else if (!slowing && worldTime != normalTime)
        {
            worldTime = Mathf.Lerp(worldTime, normalTime, timeChangeSpeed);
            if (worldTime > 0.95)
                worldTime = 1;
        }
    }

    public void SlowTime()
    {
        //Debug.Log("Freezing Time");
        worldTime = slowedTime;
    }

    public void StartTime()
    {
        //Debug.Log("Starting Time");
        worldTime = normalTime;
    }

    private void ToggleSlow(InputAction.CallbackContext ctx)
    {
        slowing = !slowing;
    }

}
