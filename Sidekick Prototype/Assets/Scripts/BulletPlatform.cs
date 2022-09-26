using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BulletPlatform : MonoBehaviour
{
    public bool enableBP = false;

    private Collider c;
    private float currTime;
    private bool bulletMode = true;

    private int defLayer;
    private string platformLayer = "Ground";

    PlayerControls controller;
    InputAction toggleBP;

    private void Awake()
    {
        controller = new PlayerControls();
        toggleBP = controller.Player.TogglePlatformsDEBUG;
        toggleBP.performed += ToggleBPTest;
        toggleBP.Enable();


        c = GetComponent<Collider>();
        defLayer = this.gameObject.layer;

        if (currTime <= 0)
            PlatformMode();

        enableBP = false;
    }

    private void OnDisable()
    {
        toggleBP.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (!enableBP && !bulletMode)
        {
            BulletMode();
            return;
        }
        else if (!enableBP)
            return;

        currTime = TimeManager.worldTime;

        if(currTime <= 0 && bulletMode)
        {
            PlatformMode();
        }
        else if (currTime > 0 && !bulletMode)
        {
            BulletMode();
        }
    }

    private void PlatformMode()
    {
        gameObject.layer = LayerMask.NameToLayer(platformLayer);


        bulletMode = false;
        c.isTrigger = false;
    }

    private void BulletMode()
    {
        gameObject.layer = defLayer;
        bulletMode = true;
        c.isTrigger = true;
    }

    private void ToggleBPTest(InputAction.CallbackContext ctx)
    {
        enableBP = !enableBP;

        //if (enableBP)
        //{
        //    if (testSlowNote != null)
        //    {
        //        testSlowNote.text = "Slow mode enabled";
        //    }
        //}
        //else
        //{
        //    if (testSlowNote != null)
        //    {
        //        testSlowNote.text = "";
        //    }
        //}
    }
}
