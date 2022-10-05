using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [Tooltip("How long should the spawn take")]
    [SerializeField] private float spawnDelay;
    [Tooltip("What should prevent this spawnpoint from being usable")]
    [SerializeField] private LayerMask overlapMask;
    [Tooltip("Override - How long should this spawnpoint wait until canceling enemy spawn")]
    [SerializeField] private float spawnOverrideDelay;
    [Tooltip("The lower and upper bound for distance between player when spawning")]
    [SerializeField] private Vector2 distanceRange;
    private ParticleSystem spawnParticles;
    /// <summary>
    /// Enemy for this spawn point to spawn
    /// </summary>
    private GameObject enemyStorage;
    /// <summary>
    /// Whether or not the spawn point is overlapped by a living enemy
    /// </summary>
    private bool overlapped;
    /// <summary>
    /// Whether or not this spawnpoint should ignore its rules and start spawning. Failsafe
    /// </summary>
    private bool overrideSpawn;
    /// <summary>
    /// Whether or not this spawn point is currently spawning an enemy
    /// </summary>
    private bool spawning;

    /// <summary>
    /// Time tracker for spawn delay
    /// </summary>
    private float spawnDelayTimer;
    /// <summary>
    /// Time tracker for override delay
    /// </summary>
    private float spawnOverrideTimer;

    /// <summary>
    /// Current spawn coroutine
    /// </summary>
    private Coroutine spawnRoutine;
    private Transform p;
    private Light spawnLight;
    private AudioSource s;
    [SerializeField] private AudioClip spawnSound;

    [Tooltip("What enemies are allowed to spawn on this spawnpoint. Drag enemy prefabs here.")]
    [SerializeField] private GameObject[] enemyWhitelist;

    private void Awake()
    {
        p = FindObjectOfType<PlayerControllerRB>().transform;
        spawnLight = GetComponentInChildren<Light>();
        spawnParticles = GetComponentInChildren<ParticleSystem>();
        s = gameObject.AddComponent<AudioSource>();
        
        if(spawnSound != null)
            s.clip = spawnSound;
    }

    /// <summary>
    /// Load an enemy into this spawn point
    /// </summary>
    /// <param name="enemy">Enemy to load into spawnpoint</param>
    /// <returns>Whether or not an enemy could be loaded into spawnpoint</returns>
    public void LoadSpawn(GameObject enemy)
    {
        if (enemyStorage == null)
        {
            ResetSpawnPoint();
            enemyStorage = enemy;
        }
        else
        {
            Debug.LogError("Spawnpoint told to load enemy when already filled!");
        }
    }

    public bool Open(GameObject proposedEnemy)
    {
        if (enemyStorage != null)
            return false;

        bool _allowed = false;
        if(enemyWhitelist.Length > 0)
        {
            EnemyBase proposed = proposedEnemy.GetComponent<EnemyBase>();
            foreach(GameObject type in enemyWhitelist)
            {
                if (type.GetComponent<EnemyBase>() == proposed)
                    _allowed = true;
            }

            return (_allowed && enemyStorage == null);
        }
        else
        {
            return true;
        }
    }

    private void ResetSpawnPoint()
    {
        spawnOverrideTimer = 0;
        overrideSpawn = false;
        spawnDelayTimer = 0;
    }

    private bool CheckDist()
    {
        float dist = Vector3.Distance(p.position, transform.position);

        return (dist >= distanceRange.x && dist <= distanceRange.y);
    }

    private void Update()
    {
        if (spawnDelayTimer <= spawnDelay)
        {
            spawnDelayTimer += Time.deltaTime * TimeManager.worldTime;
        }
        if(!spawning && spawnOverrideTimer < spawnOverrideDelay && spawnOverrideDelay != 0)
        {
            spawnOverrideTimer += Time.deltaTime * TimeManager.worldTime;
        }
        else if(!spawning && spawnOverrideTimer >= spawnOverrideDelay && enemyStorage != null)
        {
            // Do something here to fix potential 'ghost spawns', where player never reaches the point to spawn
            // maybe return to spawner? 
            //overrideSpawn = true;
            FindObjectOfType<SpawnManager>().ReturnEnemy(enemyStorage);
            enemyStorage = null;
        }

        overlapped = Physics.CheckCapsule(transform.position, transform.position + Vector3.up * 1, 0.5f, overlapMask);

        // TODO - change to normal camera grab once 'perspective cheats' are gone

        if ((!overlapped 
            && Camera.main.GetComponent<CamTarget>().InCamVision(transform.position)
            && CheckDist()) 
            || overrideSpawn)
        {
            if (!spawning && enemyStorage != null)
            {
                spawning = true;
                spawnRoutine = StartCoroutine(SpawnEnemy());
            }
        }
    }

    /// <summary>
    /// Spawn an enemy. Handle spawning and visuals here
    /// </summary>
    private IEnumerator SpawnEnemy()
    {
        if (spawnSound != null)
            s.Play();

        spawnLight.enabled = true;
        spawnParticles.Play();
        spawnDelayTimer = 0;

        while (spawnDelayTimer < spawnDelay)
            yield return null;

        Instantiate(enemyStorage, transform.position, Quaternion.identity);
        FindObjectOfType<SpawnManager>().SpawnedEnemy();

        enemyStorage = null;
        spawnRoutine = null;
        spawnLight.enabled = false;
        spawnParticles.Stop();
        if (spawnSound != null)
            s.Stop();
        spawning = false;
    }

    private void OnDisable()
    {
        if (spawnRoutine != null)
            StopCoroutine(spawnRoutine);
    }
}
