using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this is enemy lmao
/// </summary>
public class Enemy : MonoBehaviour
{
    public float rotationSpeed;
    public float walkSpeed;
    public GameObject shotPrefab;
    public float shootTime;

    public float t = 0;
    public float currTime = 1;

    [Tooltip("hello val1")]
    [SerializeField] private int testVal1;

    /// <summary>
    /// hello val2
    /// </summary>
    [SerializeField] private int testVal2;

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
        testVal1 = 2;
    }

    private void Shoot()
    {
        Instantiate(shotPrefab, transform.position, transform.rotation);

    }
}
