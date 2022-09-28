using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootStyleSwap : MonoBehaviour
{
    public bool projectile;

    public GameObject projGun;
    public GameObject hsGun;

    private PlayerControls c;
    private InputAction toggle;

    private void Awake()
    {
        projectile = true;

        c = new PlayerControls();
        toggle = c.Player.ShootToggle;
        toggle.performed += TogglePerspective;
        toggle.Enable();
    }

    private void OnDisable()
    {
        toggle.Disable();
    }

    public void TogglePerspective(InputAction.CallbackContext ctx)
    {
        projectile = !projectile;

        if (projectile)
            SetProj();
        else
            SetHS();
    }

    private void SetProj()
    {
        projGun.gameObject.SetActive(true);

        hsGun.gameObject.SetActive(false);
    }

    private void SetHS()
    {
        hsGun.gameObject.SetActive(true);

        projGun.gameObject.SetActive(false);
    }


}
