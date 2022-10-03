using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EProjectile : MonoBehaviour
{
    public float projectileSpeed;
    public int damage;
    float currTime;
    public float lifeTime;
    float t = 0;

    private AudioSource s;
    public AudioClip sound;

    // Start is called before the first frame update
    void Start()
    {
        currTime = TimeManager.worldTime;
        s = gameObject.AddComponent<AudioSource>();

        Vector3 dir = (FindObjectOfType<PlayerControllerRB>().transform.position - transform.position).normalized;
        dir += Vector3.forward * (Vector3.Distance(FindObjectOfType<PlayerControllerRB>().transform.position, transform.position) / 2);

        if (sound != null)
            AudioSource.PlayClipAtPoint(sound, dir, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        currTime = TimeManager.worldTime;

        transform.position += transform.forward * projectileSpeed * Time.deltaTime * currTime;

        if (t >= lifeTime)
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
        if (other.CompareTag("Player"))
        {
            other.GetComponent<IDamagable>().TakeDamage(damage);
        }

        Destroy(this.gameObject);
    }
}
