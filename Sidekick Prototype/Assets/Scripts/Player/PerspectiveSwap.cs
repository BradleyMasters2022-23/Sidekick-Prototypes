using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PerspectiveSwap : MonoBehaviour
{
    public bool thirdperson;

    public Camera tpCam;
    public Camera fpCam;

    private PlayerControls c;
    private InputAction toggle;

    private void Awake()
    {
        // TODO link to cheat manager
        thirdperson = true;

        c = new PlayerControls();
        toggle = c.Player.CamToggle;
        toggle.performed += TogglePerspective;
        toggle.Enable();
    }

    private void OnDisable()
    {
        toggle.Disable();
    }

    public void TogglePerspective(InputAction.CallbackContext ctx)
    {
        thirdperson = !thirdperson;

        if (thirdperson)
            SetTP();
        else
            SetFP();
    }

    private void SetTP()
    {
        tpCam.gameObject.SetActive(true);

        fpCam.gameObject.SetActive(false);
    }

    private void SetFP()
    {
        fpCam.gameObject.SetActive(true);

        tpCam.gameObject.SetActive(false);
    }
}
