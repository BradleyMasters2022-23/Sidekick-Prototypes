using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeHitscan : MonoBehaviour
{
    GameObject hitVFX;
    //RaycastHit shot;
    int damage;
    float currTime;
    [Tooltip("At what time should this bullet fire"), Range(0.01f, 1)]
    [SerializeField] private float fireThreshold = 0.1f;
    public LayerMask layersToIgnore;

    private AudioSource s;
    public AudioClip sound;

    private void Start()
    {
        s = gameObject.AddComponent<AudioSource>();
    }

    public void PrepareBullet(int dmg, GameObject vfx)
    {
        damage = dmg;
        hitVFX = vfx;
    }

    void Update()
    {
        currTime = TimeManager.worldTime;

        if (currTime >= fireThreshold)
            Fire();
    }

    private void Fire()
    {
        //IDamagable target;

        //if (prepedShot.collider.TryGetComponent<IDamagable>(out target))
        //    target.TakeDamage(damage);

        //if (hitVFX != null)
        //    Instantiate(hitVFX, prepedShot.point, Quaternion.identity);

        RaycastHit shot;

        if (Physics.Raycast(transform.position, transform.forward, out shot, Mathf.Infinity, ~layersToIgnore))
        {
            IDamagable hitTar;

            if (shot.collider.TryGetComponent<IDamagable>(out hitTar))
            {
                if (sound != null)
                    AudioSource.PlayClipAtPoint(sound, transform.position);
                hitTar.TakeDamage(damage);
            }
                

            if (hitVFX != null)
                Instantiate(hitVFX, shot.point, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
