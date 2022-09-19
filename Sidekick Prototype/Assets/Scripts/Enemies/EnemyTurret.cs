using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// this is enemy lmao
/// </summary>
public class EnemyTurret : IDamagable
{
    public enum EnemyState
    {
        Idle,
        Attacking
    }

    [SerializeField] private EnemyState state;

    public GameObject turretPoint;
    public Transform[] shootPoints;

    [Range(0f, 1f)]
    public float rotationSpeed;
    public GameObject shotPrefab;
    public float shootTime;

    [Tooltip("Range the enemy can attack from")]
    public float attackRange;
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
        p = FindObjectOfType<PlayerControllerRB>().gameObject;
        
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        CheckState();
        // Get current time state
        currTime = TimeManager.worldTime;
        currDist = Vector3.Distance(p.transform.position, transform.position);

        switch (state)
        {
            case EnemyState.Idle:
                {

                    break;
                }
            case EnemyState.Attacking:
                {
                    // Shoot every few seconds if in range
                    if (time >= shootTime)
                    {
                        foreach(Transform t in shootPoints)
                        {
                            Shoot(t);
                        }

                        time = 0;
                    }
                    else if (time < shootTime)
                    {
                        time += Time.deltaTime * currTime;
                    }

                    break;
                }
        }

        // Get direction of player, rotate towards them
        Vector3 direction = (p.transform.position - transform.position);
        Quaternion rot = Quaternion.LookRotation(direction);

        float nextXAng = Mathf.LerpAngle(turretPoint.transform.localRotation.eulerAngles.x, rot.eulerAngles.x, rotationSpeed * currTime);
        float nextYAng = Mathf.LerpAngle(transform.rotation.eulerAngles.y, rot.eulerAngles.y, rotationSpeed * currTime);

        turretPoint.transform.localRotation = Quaternion.Euler(nextXAng, 0, 0);
        transform.rotation = Quaternion.Euler(0, nextYAng, 0);
    }

    /// <summary>
    /// Check state, see if it should swap
    /// </summary>
    /// <returns>New state</returns>
    private void CheckState()
    {
        switch (state)
        {
            case EnemyState.Idle:
                {
                    // Switch to attacking if line of sight and within ideal range
                    if(LineOfSight(p) && currDist <= attackRange)
                    {
                        state = EnemyState.Attacking;

                    }

                    break;
                }
            case EnemyState.Attacking:
                {
                    // Resume moving if outside of attack range or rounded corner
                    if (!LineOfSight(p))
                    {
                        state = EnemyState.Idle;
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
        Vector3 direction = target.transform.position - transform.position;
        
        // Set mask to ignore raycasts and enemy layer
        int lm = LayerMask.NameToLayer("Enemy");
        lm = (1 << lm);
        lm |= (1 << LayerMask.NameToLayer("Ignore Raycast"));

        // Try to get player
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, attackRange, ~lm))
        {
            if (hit.transform.CompareTag("Player"))
                return true;
        }
        return false;
    }


    private bool InVision()
    {
        Vector3 temp = (p.transform.position - transform.position).normalized;
        float angle = Vector3.SignedAngle(temp, transform.forward, Vector3.up);
        return (Mathf.Abs(angle) <= lookRadius);
    }

    private void Shoot(Transform point)
    {
        GameObject o = Instantiate(shotPrefab, point.transform.position, point.transform.rotation);
    }
}
