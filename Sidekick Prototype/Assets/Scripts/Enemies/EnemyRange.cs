using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// this is enemy lmao
/// </summary>
public class EnemyRange : EnemyBase
{

    [SerializeField] private EnemyState state; 

    private NavMeshAgent agent;

    [Range(0f, 1f)]
    public float rotationSpeed;
    public float walkSpeed;
    public GameObject shotPrefab;
    public float shootTime;

    [Tooltip("Range the enemy can attack from")]
    public float attackRange;
    [Tooltip("Range the enemy will try to stay around")]
    public float idealRange;
    [Tooltip("How close to the player before moving away")]
    public float moveThreshold;
    private float currDist;

    public int lookRadius;

    private float time = 0;
    private float currTime = 1;
    private GameObject p;
    public float overshootMult;

    // Start is called before the first frame update
    void Start()
    {
        currTime = TimeManager.worldTime;
        agent = GetComponent<NavMeshAgent>();
        p = FindObjectOfType<PlayerControllerRB>().gameObject;
        
        agent.speed = walkSpeed;
        agent.updateRotation = false;
        state = EnemyState.Moving;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        CheckState();
        // Get current time state
        currTime = TimeManager.worldTime;
        currDist = Vector3.Distance(p.transform.position, transform.position);
        agent.speed = walkSpeed * currTime;

        switch (state)
        {
            case EnemyState.Moving:
                {
                    // Move towards player position

                    agent.speed = walkSpeed * currTime;
                    agent.SetDestination(p.transform.position);

                    break;
                }
            case EnemyState.Attacking:
                {
                    // Shoot every few seconds if in range
                    if (time >= shootTime && InVision())
                    {
                        Shoot();
                        time = 0;
                    }
                    else if (time < shootTime)
                    {
                        time += Time.deltaTime * currTime;
                    }

                    // run away from player when too close
                    if(Mathf.Abs(currDist - idealRange) > moveThreshold && currDist < idealRange)
                    {
                        time = 0;

                        Vector3 awayDir = ((transform.position + Vector3.up) - p.transform.position).normalized * walkSpeed/2;
                        agent.SetDestination(transform.position + awayDir);
                    }

                    break;
                }
        }

        // Get direction of player, rotate towards them
        Vector3 direction = p.transform.position - (transform.position+Vector3.up);
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float nextAng = Mathf.LerpAngle(transform.rotation.eulerAngles.y, angle, rotationSpeed * currTime);
        transform.rotation = Quaternion.Euler(0, nextAng, 0);
    }

    /// <summary>
    /// Check state, see if it should swap
    /// </summary>
    /// <returns>New state</returns>
    private void CheckState()
    {
        switch (state)
        {
            case EnemyState.Moving:
                {
                    // Switch to attacking if line of sight and within ideal range
                    if(LineOfSight(p) && currDist <= idealRange)
                    {
                        state = EnemyState.Attacking;

                        // Overshoot a tiny bit, namely corners
                        Vector3 temp = agent.velocity.normalized;
                        temp *= overshootMult;

                        agent.SetDestination((transform.position + Vector3.up) + temp);
                    }

                    break;
                }
            case EnemyState.Attacking:
                {
                    // Resume moving if outside of attack range or rounded corner
                    if (!LineOfSight(p))
                    {
                        state = EnemyState.Moving;
                        time = shootTime/2;
                    }

                    break;
                }
        }
    }
    
    /// <summary>
    /// Check if this object has line of sight on target via raycast
    /// </summary>
    /// <param name="target">target to check</param>
    /// <returns>Line of sight</returns>
    public bool LineOfSight(GameObject target)
    {
        Vector3 direction = target.transform.position - (transform.position + Vector3.up);
        
        // Set mask to ignore raycasts and enemy layer
        int lm = LayerMask.NameToLayer("Enemy");
        lm = (1 << lm);
        lm |= (1 << LayerMask.NameToLayer("Ignore Raycast"));

        // Try to get player
        RaycastHit hit;
        if (Physics.Raycast((transform.position + Vector3.up), direction, out hit, attackRange, ~lm))
        {
            if (hit.transform.CompareTag("Player"))
                return true;
        }
        return false;
    }


    private bool InVision()
    {
        Vector3 temp = (p.transform.position - (transform.position + Vector3.up)).normalized;
        float angle = Vector3.SignedAngle(temp, transform.forward, Vector3.up);
        return (Mathf.Abs(angle) <= lookRadius);
    }

    private void Shoot()
    {
        GameObject o = Instantiate(shotPrefab, (transform.position + Vector3.up), shotPrefab.transform.rotation);
        o.transform.LookAt(p.transform);

    }

    protected override void Awake()
    {
        //GetComponent<Renderer>().material.color = Color.red;
        base.Awake();

        // If dummy cannot be killed, make sure to remove from pool
        //if (invulnerable)
        //    FindObjectOfType<DoorManager>().DestroyEnemy();
    }


    public override void Die()
    {
        base.Die();
        //FindObjectOfType<DoorManager>().DestroyEnemy();
        FindObjectOfType<SpawnManager>().DestroyEnemy();
    }
}
