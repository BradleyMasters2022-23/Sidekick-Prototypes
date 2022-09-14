using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float rotationSpeed;
    public float walkSpeed;
    public GameObject shotPrefab;
    public float shootTime;

    public float t = 0;
    public float currTime = 1;


    // Start is called before the first frame update
    void Start()
    {
        currTime = TimeManager.worldTime;
    }

    // Update is called once per frame
    void Update()
    {
        currTime = TimeManager.worldTime;

        transform.Translate(transform.forward * walkSpeed * Time.deltaTime * currTime);
        transform.Rotate(new Vector3(0, rotationSpeed, 0) * Time.deltaTime * currTime);


        if(t >= shootTime)
        {
            Shoot();
            t = 0;
        }
        else
        {
            t += Time.deltaTime * currTime;
        }

    }

    private void Shoot()
    {
        Instantiate(shotPrefab, transform.position, transform.rotation);

    }
}
