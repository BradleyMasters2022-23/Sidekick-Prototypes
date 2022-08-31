using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PProjectile : MonoBehaviour
{
    public float projectileSpeed;

    public bool freezePlayerP = false;
    public float freezeSpawnDist;

    float currTime;

    Rigidbody rb;

    public void Prep(Vector3 pos, Vector3 rot)
    {
        transform.position = pos;
        transform.rotation = Quaternion.Euler(rot);
    }

    // Start is called before the first frame update
    void Start()
    {
        currTime = TimeManager.worldTime;

        rb = GetComponent<Rigidbody>();

        //rb.AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);

        if(freezePlayerP && currTime == 0)
        {
            transform.position += (transform.forward * freezeSpawnDist);
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
    }
}
