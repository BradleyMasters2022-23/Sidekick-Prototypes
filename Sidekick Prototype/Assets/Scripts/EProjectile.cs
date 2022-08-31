using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EProjectile : MonoBehaviour
{
    public float projectileSpeed;
    float currTime;
    //Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        currTime = TimeManager.worldTime;
        //rb.AddForce(transform.forward * projectileSpeed * TimeManager.worldTime);
    }

    // Update is called once per frame
    void Update()
    {
        currTime = TimeManager.worldTime;

        transform.position += transform.forward * projectileSpeed * Time.deltaTime * currTime;
    }
}
