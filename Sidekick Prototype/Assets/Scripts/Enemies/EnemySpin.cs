using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// this is enemy lmao
/// </summary>
public class EnemySpin : EnemyBase
{
    public Transform[] shootPoints;
    private Animator a;

    [Range(0f, 3f)]
    public float moveSpeed;
    public GameObject shotPrefab;
    public float shootTime;

    private float currTime = 1;
    private float shootTimer = 0;

    protected override void Awake()
    {
        //GetComponent<Renderer>().material.color = Color.red;
        a = GetComponent<Animator>();
        base.Awake();

        // If there is a spawner, add this to the counter
        SpawnManager spawnManager = FindObjectOfType<SpawnManager>();
        if(spawnManager != null)
        {
            spawnManager.AddEnemy();
        }

        a.speed = moveSpeed;

        // If dummy cannot be killed, make sure to remove from pool
        if (invulnerable)
            FindObjectOfType<SpawnManager>().DestroyEnemy();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        currTime = TimeManager.worldTime;

        a.SetFloat("currTime", currTime);

        if (shootTimer >= shootTime)
        {
            Shoot();
            shootTimer = 0;
        }
        else if (shootTimer < shootTime)
        {
            shootTimer += Time.deltaTime * currTime;
        }

    }

    private void Shoot()
    {
        foreach (Transform t in shootPoints)
        {
            Instantiate(shotPrefab, t.transform.position, t.transform.rotation);
        }
    }

    public override void Die()
    {
        base.Die();
        FindObjectOfType<SpawnManager>().DestroyEnemy();
    }
}