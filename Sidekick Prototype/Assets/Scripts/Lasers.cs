using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lasers : IRangeAttack
{
    [Header("---Gameplay---")]
    [Tooltip("How long does it hold its shot and show the 'flare' before firing")]
    [SerializeField] private float fireDelay;
    [Tooltip("How close to firing should this allow to fire early when time is frozen")]
    [SerializeField] private float earlyFireThreshold;
    [Tooltip("What is the cooldown for taking damage while standing inside the laser in frozen time")]
    [SerializeField] private float damageRate;
    [Tooltip("What is the damage per tick when standing in laser in frozen time")]
    [SerializeField] private float frozenDamageModifier;
    [Tooltip("If missed, what is the default laser distance")]
    [SerializeField] private float defaultDistance;
    [Tooltip("What physics layer can this laser hit")]
    [SerializeField] private LayerMask hitLayers;
    [Tooltip("What is the cooldown for taking damage while standing inside the laser in frozen time")]
    [SerializeField] private float laserVisualOvershoot;
    
    [Header("---Audio---")]
    [Tooltip("Sound played when starting to aim")]
    [SerializeField] private AudioClip aimSound;
    [Tooltip("Sound played when firing")]
    [SerializeField] private AudioClip shootSound;

    private float lifeTimer;
    private float delayTimer;
    private float damageRateTimer;
    private bool hitTarget;
    private bool fired;

    /// <summary>
    /// Visual line renderer for visuals
    /// </summary>
    private LineRenderer laserShot;
    /// <summary>
    /// Origin of the laser shot
    /// </summary>
    private Vector3 origin;
    /// <summary>
    /// Direction the laser shot fires
    /// </summary>
    private Vector3 direction;

    private void Start()
    {
        hitTarget = false;

        GameObject p = FindObjectOfType<PlayerControllerRB>().gameObject;
        if (aimSound != null)
            AudioSource.PlayClipAtPoint(aimSound, FindObjectOfType<PlayerControllerRB>().transform.position, 1f);

        Vector3 t = p.transform.position - transform.position;
        laserShot = GetComponent<LineRenderer>();
        laserShot.enabled = false;
        PrepareProjectile(transform.position, transform.forward);
    }

    /// <summary>
    /// Load in the origin and direction
    /// </summary>
    /// <param name="_origin">origin point of this shot</param>
    /// <param name="_direction">direction of this shot</param>
    private void PrepareProjectile(Vector3 _origin, Vector3 _direction)
    {
        origin = _origin;
        direction = _direction;
    }

    /// <summary>
    /// Fire the laser and prepare visuals and sounds
    /// </summary>
    private void FireLaser()
    {
        if (aimSound != null)
            AudioSource.PlayClipAtPoint(shootSound, FindObjectOfType<PlayerControllerRB>().transform.position, 1f);

        fired = true;
        RaycastHit hit;
        transform.rotation = Quaternion.LookRotation(direction);
        laserShot.SetPosition(0, origin);

        if (Physics.Raycast(origin, direction, out hit, defaultDistance, hitLayers))
        {
            laserShot.SetPosition(1, hit.point + direction * laserVisualOvershoot);
            IDamagable target;
            if(!hitTarget && hit.collider.TryGetComponent<IDamagable>(out target))
            {
                target.TakeDamage(damage);
                hitTarget = true;
            }
        }
        else
        {
            laserShot.SetPosition(1, origin + direction*defaultDistance);
        }

        laserShot.enabled = true;
    }

    /// <summary>
    /// Fire constantly without visual indicator changes
    /// </summary>
    private void FireFrozenTime()
    {
        if(damageRateTimer >= damageRate)
        {
            hitTarget = false;
            damageRateTimer = 0;
        }
        else
        {
            damageRateTimer += Time.deltaTime;
        }


        RaycastHit hit;

        if (Physics.Raycast(origin, direction, out hit, defaultDistance, hitLayers))
        {
            IDamagable target;
            if (!hitTarget && hit.collider.TryGetComponent<IDamagable>(out target))
            {
                target.TakeDamage((int)(damage * frozenDamageModifier));
                hitTarget = true;
            }
        }
    }

    /// <summary>
    /// Manage timers
    /// </summary>
    private void Update()
    {
        if(!fired && delayTimer >= fireDelay)
        {
            FireLaser();
        }
        else if (!fired)
        {
            delayTimer += Time.deltaTime * TimeManager.worldTime;

            // If within the threshold and time has frozen, then fire early anyways
            if(TimeManager.worldTime == 0 && Mathf.Abs(fireDelay - delayTimer) <= earlyFireThreshold)
            {
                Debug.Log("Frozen threshold reached");
                FireLaser();
            }
        }


        if(fired && lifeTimer >= lifeTime)
        {
            DestroyLaser();
        }
        else if(fired && TimeManager.worldTime > .8)
        {
            lifeTimer += Time.deltaTime * TimeManager.worldTime;
        }

        if(fired && TimeManager.worldTime < 1)
        {
            FireFrozenTime();
        }
    }

    /// <summary>
    /// Destroys the laser shot
    /// </summary>
    private void DestroyLaser()
    {
        // Add additional effects for destroying the laser
        laserShot.enabled = false;
        GetComponent<Collider>().enabled = false;

        Destroy(this.gameObject);
    }
}
