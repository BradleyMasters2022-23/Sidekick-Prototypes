using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

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
    InputAction toggleATest;
    InputAction toggleBTest;
    InputAction toggleBP;
    InputAction togglePerspective;
    InputAction toggleHitscan;

    public bool enableBP;
    public bool enableFirstPerson;
    public bool enableHitscan;

    bool slowing;

    [Header("Refill management")]
    [SerializeField] private Slider slider;
    public bool unlimited;
    public float maxGauge;
    public float decreaseRate;
    public float refillRate;
    private float currGauge;
    private bool emptied = false;
    public float refillDelay;
    public float emptiedDelay;
    
    private float emptiedTimer;
    private float refillTimer;

    public Image fillImg;
    public Color emptiedColor;
    private Color defColor;

    private bool testSlow = false;
    private bool testUnlimitedSlow = false;

    public TextMeshProUGUI testSlowNote;
    public TextMeshProUGUI testUnlimitedNote;
    public TextMeshProUGUI testBPNote;
    public TextMeshProUGUI testPerspectiveNote;
    public TextMeshProUGUI testHitscanNote;

    private AudioSource s;
    public AudioClip stopSFX;
    public AudioClip startSFX;

    private void Start()
    {
        s = gameObject.AddComponent<AudioSource>();

        controller = new PlayerControls();
        slowing = false;

        currGauge = maxGauge;
        slider.maxValue = currGauge;
        slider.value = maxGauge;
        defColor = fillImg.color;
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

        toggleATest = controller.Player.ToggleSlowDEBUG;
        toggleATest.performed += ToggleATest;
        toggleATest.Enable();

        toggleBTest = controller.Player.ToggleUnlimitedSlowDEBUG;
        toggleBTest.performed += ToggleBTest;
        toggleBTest.Enable();

        toggleBP = controller.Player.TogglePlatformsDEBUG;
        toggleBP.performed += ToggleBPTest;
        toggleBP.Enable();

        togglePerspective = controller.Player.CamToggle;
        togglePerspective.performed += TogglePerspectiveNote;
        togglePerspective.Enable();

        toggleHitscan = controller.Player.ShootToggle;
        toggleHitscan.performed += ToggleHitscanNote;
        toggleHitscan.Enable();
    }


    private void OnDisable()
    {
        toggleATest.Disable();
        toggleBTest.Disable();
        toggleBP.Disable();
        togglePerspective.Disable();
        toggleHitscan.Disable();
        if (toggle)
        {
            slow.Disable();
        }
        else
        {
            if(!emptied)
                slowHold.Disable();
        }
    }

    private void FixedUpdate()
    {
        if(slowing)
        {
            refillTimer = 0;
        }


        if (!emptied)
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
        else
        {
            if (!slowing && worldTime != normalTime)
            {
                worldTime = Mathf.Lerp(worldTime, normalTime, timeChangeSpeed);
                if (worldTime > 0.95)
                    worldTime = 1;
                
            }

            if (emptiedTimer >= emptiedDelay)
            {
                fillImg.color = defColor;
                emptied = false;
                emptiedTimer = 0;
                slowHold.Enable();
            }
            else
            {
                emptiedTimer += Time.deltaTime;
            }
        }

        if (slowing)
        {
            DecreaseVal();
        }
        else if (!slowing || emptied)
        {
            if (refillTimer >= refillDelay)
            {
                IncreaseVal();
            }
            else
            {
                refillTimer += Time.deltaTime;
            }
        }
    }

    public void SlowTime()
    {
        worldTime = slowedTime;
    }

    public void StartTime()
    {
        worldTime = normalTime;
    }

    private void ToggleSlow(InputAction.CallbackContext ctx)
    {
        if(!emptied)
        {
            slowing = !slowing;

            if (slowing)
                s.PlayOneShot(stopSFX, 0.7f);
            else
            {
                s.PlayOneShot(startSFX);
            }
        }
    }

    public void DecreaseVal()
    {
        if (unlimited)
            return;

        if(currGauge - decreaseRate <= 0)
        {
            currGauge = 0;
            slowing = false;
            emptied = true;
            fillImg.color = emptiedColor;
            slowHold.Disable();
            s.PlayOneShot(startSFX);
        }
        else
        {
            currGauge -= decreaseRate;
        }

        slider.value = currGauge;
    }

    public void IncreaseVal()
    {
        if (currGauge + refillRate >= maxGauge)
        {
            currGauge = maxGauge;
        }
        else
        {
            currGauge += decreaseRate;
        }

        slider.value = currGauge;
    }

    private void ToggleATest(InputAction.CallbackContext ctx)
    {
        if (slowing)
            return;

        testSlow = !testSlow;

        if (testSlow)
        {
            slowedTime = 0.1f;
            if(testSlowNote != null)
            {
                testSlowNote.text = "Slow mode enabled";
            }
        }
        else
        {
            slowedTime = 0f;
            if (testSlowNote != null)
            {
                testSlowNote.text = "";
            }
        }
    }

    private void ToggleBTest(InputAction.CallbackContext ctx)
    {
        if (slowing)
            return;

        testUnlimitedSlow = !testUnlimitedSlow;

        unlimited = !unlimited;

        if (testUnlimitedSlow && testUnlimitedNote != null)
        {
            testUnlimitedNote.text = "Unlimited time enabled";
        }
        else if(testUnlimitedNote != null)
        {
            testUnlimitedNote.text = "";
        }
    }


    private void ToggleBPTest(InputAction.CallbackContext ctx)
    {
        enableBP = !enableBP;

        if (enableBP)
        {
            if (testBPNote != null)
            {
                testBPNote.text = "Bullet Platforms Enabled";
            }
        }
        else
        {
            if (testBPNote != null)
            {
                testBPNote.text = "";
            }
        }
    }

    private void TogglePerspectiveNote(InputAction.CallbackContext ctx)
    {
        enableFirstPerson = !enableFirstPerson;
        
        if (enableFirstPerson)
        {
            if (testPerspectiveNote != null)
            {
                testPerspectiveNote.text = "First person enabled";
            }
        }
        else
        {
            if (testPerspectiveNote != null)
            {
                testPerspectiveNote.text = "";
            }
        }
    }

    private void ToggleHitscanNote(InputAction.CallbackContext ctx)
    {
        enableHitscan = !enableHitscan;
        
        if (enableHitscan)
        {
            if (testHitscanNote != null)
            {
                testHitscanNote.text = "Hitscan mode enabled";
            }
        }
        else
        {
            if (testHitscanNote != null)
            {
                testHitscanNote.text = "";
            }
        }
    }
}
