using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this is enemy lmao
/// </summary>
public class Enemy : IDamagable
{
    [Range(0,1)]
    public float rotationSpeed;
    public float walkSpeed;
    public GameObject shotPrefab;
    public float shootTime;

    [Tooltip("Range the enemy can attack from")]
    public float attackRange;
    [Tooltip("Range the enemy will try to stay around")]
    public float idealRange;
    [Tooltip("Threshold to move the enemy")]
    public float moveThreshold;
    private float currDist;

    private float time = 0;
    private float currTime = 1;
    private GameObject p;



    // Start is called before the first frame update
    void Start()
    {
        currTime = TimeManager.worldTime;
        p = FindObjectOfType<PlayerControllerRB>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        currDist = Vector3.Distance(p.transform.position, transform.position);

        // Shoot every few seconds if in range
        if (time >= shootTime && currDist  <= attackRange)
        {
            Shoot();
            time = 0;
        }
        else if(time < shootTime)
        {
            time += Time.deltaTime * currTime;
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        // Get current time state
        currTime = TimeManager.worldTime;

        // Get direction of player
        Vector3 direction = p.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        // Rotate enemy towards player
        float nextAng = Mathf.LerpAngle(transform.rotation.eulerAngles.y, angle, rotationSpeed * currTime);
        transform.rotation = Quaternion.Euler(0, nextAng, 0);

        // Move enemy
        if (Mathf.Abs(currDist - idealRange) > moveThreshold)
        {
            if (currDist < idealRange)
            {
                transform.position += transform.forward * -1 * walkSpeed * Time.deltaTime * currTime;
            }
            else if (currDist > idealRange)
            {
                transform.position += transform.forward * walkSpeed * Time.deltaTime * currTime;
            }
        }
    }
    
    private void Shoot()
    {
        Instantiate(shotPrefab, transform.position, transform.rotation);

    }
}
