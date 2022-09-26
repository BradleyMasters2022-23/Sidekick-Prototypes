using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGun : MonoBehaviour
{
    private bool firing;

    public CamTarget shootCam;
    public PlayerControls controller;
    private InputAction shoot;

    public Transform shootPoint;
    public GameObject bullet;


    [Tooltip("Time it takes between each shot")]
    public float fireDelay;
    private float t;


    private void Start()
    {
        firing = false;

        controller = new PlayerControls();
        shoot = controller.Player.Shoot;
        shoot.Enable();
        shoot.started += ToggleTrigger;
        shoot.canceled += ToggleTrigger;
    }

    private void OnDisable()
    {
        shoot.Disable();
    }

    private void Update()
    {
        if(t < fireDelay)
            t += Time.deltaTime;

        if(firing)
        {
            if(t >= fireDelay)
            {
                t = 0;
                Shoot();
            }
        }
    }

    private void ToggleTrigger(InputAction.CallbackContext ctx)
    {
        firing = !firing;
    }

    private void Shoot()
    {
        GameObject t = Instantiate(bullet, shootPoint.position, transform.rotation);
        t.transform.LookAt(shootCam.GetTarget());
    }

    /// <summary>
    /// Replace the players shot projectile
    /// </summary>
    /// <param name="p">projectile prefab to fire</param>
    public void LoadNewProjectile(GameObject p)
    {
        bullet = p;
    }
}
