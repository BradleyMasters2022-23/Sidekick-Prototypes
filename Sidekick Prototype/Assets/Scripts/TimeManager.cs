using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimeManager : MonoBehaviour
{
    public static float worldTime;

    [HideInInspector] public float normalTime = 1;
    [Range(0, 1)]
    public float slowedTime;
    public float timeChangeSpeed;

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
            //StartTime();
        }
        else
        {
            //SlowTime();
        }
    }

}
