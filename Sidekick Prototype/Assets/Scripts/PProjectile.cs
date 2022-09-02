using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PProjectile : MonoBehaviour
{
    public float projectileSpeed;

    public bool freezePlayerP = false;
    public float freezeSpawnDist;

    public float currTime;


    // Start is called before the first frame update
    void Start()
    {
        currTime = TimeManager.worldTime;

        //rb.AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);

        if(freezePlayerP && currTime == 0)
        {
            transform.position += transform.forward * freezeSpawnDist;
            Debug.Log("Spawn ahead done");
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
