using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGunHitscan : MonoBehaviour
{
    private bool firing;

    public CamTarget shootCam;
    public PlayerControls controller;
    private InputAction shoot;

    public int damage;
    public Transform shootPoint;
    public GameObject frozenBullet;
    public GameObject hitVFX;

    [Tooltip("Time it takes between each shot")]
    public float fireDelay;
    private float t;
    [Tooltip("What time speed should the stationary bullets fire"), Range(0, 0.99f)]
    public float freezeShotThreshold = 0.1f;

    [Tooltip("How far do bullets spawn from the bullet"), Range(0, 3)]
    public float fireOffset = 1f;

    private void Start()
    {
        firing = false;

        controller = new PlayerControls();
        shoot = controller.Player.Shoot;
        shoot.Enable();
        shoot.started += ToggleTrigger;
        shoot.canceled += ToggleTrigger;
    }
    private void OnEnable()
    {
        if(shoot != null)
            shoot.Enable();
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
        RaycastHit hit = shootCam.GetHit();

        if(TimeManager.worldTime > freezeShotThreshold)
        {
            IDamagable target;

            if(hit.collider.TryGetComponent<IDamagable>(out target))
            {
                target.TakeDamage(damage);
            }

            Instantiate(hitVFX, hit.point, Quaternion.identity);
        }
        else
        {
            GameObject t = Instantiate(frozenBullet, shootPoint.position, transform.rotation);
            t.transform.LookAt(shootCam.GetTarget());
            t.transform.position += Vector3.forward * fireOffset;
            t.GetComponent<FreezeHitscan>().PrepareBullet(hit, damage, hitVFX);
        }
    }

}
