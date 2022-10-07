using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PProjectile : MonoBehaviour
{
    public float projectileSpeed;

    public bool hitscan;

    public int damage;

    public bool freezePlayerP = false;
    public float freezeSpawnDist;

    public GameObject hitVFX;
    public GameObject hitscanBullet;

    public float currTime;

    public float lifeTime;
    private float t = 0;

    private AudioSource s;
    public AudioClip sound;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        currTime = TimeManager.worldTime;
        s = gameObject.AddComponent<AudioSource>();
        if (freezePlayerP && currTime <= .2 && transform.parent == null)
        {
            transform.position += transform.forward * freezeSpawnDist;
        }

        if (hitscan)
        {
            GameObject s = Instantiate(hitscanBullet, transform.position, transform.rotation);
            s.GetComponent<FreezeHitscan>().PrepareBullet(damage, hitVFX);
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        currTime = TimeManager.worldTime;

        if (freezePlayerP)
            transform.position += transform.forward * projectileSpeed * Time.deltaTime * currTime;
        else
            transform.position += transform.forward * projectileSpeed * Time.deltaTime;

        if(t >= lifeTime)
        {
            Destroy(this.gameObject);
        }
        else
        {
            t += Time.deltaTime * currTime;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            other.GetComponent<IDamagable>().TakeDamage(damage);
            if (sound != null)
                AudioSource.PlayClipAtPoint(sound, FindObjectOfType<PlayerControllerRB>().transform.position, 2.5f);
        }

        if (hitVFX != null)
            Instantiate(hitVFX, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    //private void Fire()
    //{
    //    RaycastHit shot; 
        
    //    if(Physics.Raycast(transform.position, transform.forward, out shot, Mathf.Infinity, ~layersToIgnore))
    //    {
    //        IDamagable hitTar;

    //        if (shot.collider.TryGetComponent<IDamagable>(out hitTar))
    //            hitTar.TakeDamage(damage);

    //        if (hitVFX != null)
    //            Instantiate(hitVFX, shot.point, Quaternion.identity);
    //    }

    //    Destroy(gameObject);
    //}
}
