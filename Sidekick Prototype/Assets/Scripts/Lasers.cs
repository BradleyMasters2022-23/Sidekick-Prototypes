using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lasers : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float lifeTime;
    [SerializeField] private float damageRate;
    [SerializeField] private int maxDamageTick;
    [SerializeField] private float defaultDistance;
    [SerializeField] private Vector3 originPoint;
    [SerializeField] private Vector3 aimDirection;

    [SerializeField] private LayerMask hitLayers;

    private float lifeTimer;
    private float damageTickTimer;


    private void Start()
    {
        GameObject p = FindObjectOfType<PlayerControllerRB>().gameObject;
        Vector3 t = p.transform.position - transform.position;

        PrepareProjectile(transform.position, t.normalized);
    }

    public void PrepareProjectile(Vector3 origin, Vector3 direction)
    {
        RaycastHit hit;
        //Debug.Log("told origin of " + origin);

        transform.rotation = Quaternion.LookRotation(direction);
        float distance;
        if (Physics.Raycast(origin, direction, out hit, defaultDistance, hitLayers))
        {
            distance = hit.distance;
        }
        else
        {
            distance = defaultDistance;
        }
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, distance);
        transform.position = origin + (direction.normalized * (transform.localScale.z / 2));
    }

    private void Update()
    {
        if(lifeTimer >= lifeTime)
        {
            DestroyLaser();
        }
        else
        {
            lifeTimer += Time.deltaTime * TimeManager.worldTime;
        }
    }

    private void DestroyLaser()
    {
        // Add additional effects for destroying the laser
        GetComponent<Collider>().enabled = false;

        Destroy(this.gameObject);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        other.GetComponent<IDamagable>().TakeDamage(damage);
    //    }

    //    Destroy(this.gameObject);
    //}

}
