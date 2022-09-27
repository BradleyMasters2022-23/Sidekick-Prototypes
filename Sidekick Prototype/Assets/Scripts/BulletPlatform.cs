using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BulletPlatform : MonoBehaviour
{
    public bool enableBP;

    private Collider c;
    private float currTime;
    private bool bulletMode;

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

        bulletMode = true;
        enableBP = FindObjectOfType<TimeManager>().enableBP;

        if (currTime <= 0 && enableBP)
            PlatformMode();
    }

    private void OnDisable()
    {
        toggleBP.Disable();
    }

    private void FixedUpdate()
    {
        currTime = TimeManager.worldTime;

        if(!enableBP && !bulletMode)
        {
            BulletMode();
            return;
        }

        if (!enableBP)
            return;


        if (currTime <= 0 && bulletMode)
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
    }
}
