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

    // Start is called before the first frame update
    void Start()
    {
        currTime = TimeManager.worldTime;
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
