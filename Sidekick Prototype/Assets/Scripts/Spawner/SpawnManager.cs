using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public WaveSO[] waveOptions;
    private WaveSO chosenWave;
    private int waveIndex;

    [Tooltip("How many seconds to wait before starting the wave?")]
    [SerializeField] private float startDelay;

    private Queue<GameObject> spawnQueue = new Queue<GameObject>();
    public int maxEnemies = 3;
    private int enemyCount = 0;
    private int waitingEnemies = 0;
    public Vector2 spawnDelay = new Vector2(0, 1);
    private bool spawning = false;
    private float spawnTimer;
    private float targetDelay;
    private bool wait = false;
    private Coroutine spawnRoutine;
    private Coroutine backupCheckRoutine;

    private AudioSource s;
    public AudioClip sound;

    private SpawnPoint[] spawnPoints;

    private float currTime;

    private bool gaveUpgrade;

    private void Start()
    {
        currTime = TimeManager.worldTime;
        gaveUpgrade = false;

        spawnPoints = FindObjectsOfType<SpawnPoint>();
        if(spawnPoints.Length <= 0)
        {
            Debug.LogError("This room has no spawnpoints set! Disabling spawner!");
            CompleteRoom();
            return;
        }
        StartCoroutine(ActivateSpawner());
        s = gameObject.AddComponent<AudioSource>();
    }

    public void AddEnemy()
    {
        enemyCount++;
    }

    public IEnumerator ActivateSpawner()
    {
        yield return new WaitForSeconds(startDelay);
        ChooseWave();
    }

    public void ChooseWave()
    {
        if(waveOptions.Length <= 0)
        {
            CompleteRoom();
            return;
        }
        else if(waveOptions.Length == 1)
        {
            chosenWave = waveOptions[0];
        }
        else
        {
            chosenWave = waveOptions[Random.Range(0, waveOptions.Length)];
        }

        spawnRoutine = StartCoroutine(SpawnNextWave());
    }

    private IEnumerator SpawnNextWave()
    {
        spawning = true;
        // Load the queue for all enemies in this wave
        foreach (GameObject enemy in chosenWave.allWaves[waveIndex].wave)
            spawnQueue.Enqueue(enemy);

        // Continually decide spawnpoints to use
        SpawnPoint _spawnPoint;
        while (spawnQueue.Count > 0 || waitingEnemies > 0 || !SpawnpointsEmpty())
        {
            int c = 0;
            // Only spawn if not past max
            if ((enemyCount+waitingEnemies) < maxEnemies)
            {
                // Spawn a new enemy with a randomized delay between the range, not choosing same point twice back to back
                do
                {
                    c++;
                    _spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                    // Backup incase they get stuck
                    //if (c >= 1000)
                    //    _spawnPoint.SetUsable();

                    yield return null;

                } while (spawnQueue.Count != 0 && !_spawnPoint.Open(spawnQueue.Peek()));
                //lastSpawn = _spawnPoint;

                if(spawnQueue.Count != 0)
                    SpawnEnemy(spawnQueue.Dequeue(), _spawnPoint);
            }

            // Delay. Done like this to take into account time freeze
            targetDelay = Random.Range(spawnDelay.x, spawnDelay.y);
            wait = true;
            while (wait)
            {
                yield return null;
            }
        }
        spawning = false;
        backupCheckRoutine = StartCoroutine(CheckCount());
    }



    private void SpawnEnemy(GameObject enemyPrefab, SpawnPoint spawnPoint)
    {
        spawnPoint.LoadSpawn(enemyPrefab);
        waitingEnemies++;
    }


    private void FixedUpdate()
    {
        currTime = TimeManager.worldTime;

        if (wait && spawnTimer >= targetDelay)
        {
            spawnTimer = 0;
            wait = false;
        }
        else if (wait)
        {
            spawnTimer += Time.deltaTime * currTime;
        }
    }

    /// <summary>
    /// Determine whether to start the next wave or complete the room
    /// </summary>
    private void WaveFinished()
    {
        if (gaveUpgrade)
            return;

        Debug.Log("Wave Finished");
        spawnRoutine = null;
        waveIndex++;

        if(waveIndex >= chosenWave.allWaves.Length)
        {
            CompleteRoom();
            spawnQueue.Clear();
        }
        else
        {
            StartCoroutine(SpawnNextWave());
        }
    }

    private void CompleteRoom()
    {
        gaveUpgrade = true;

        if (sound != null)
            s.PlayOneShot(sound);

        FindObjectOfType<DoorManager>().UnlockAllDoors();

        if(chosenWave != null)
            FindObjectOfType<RewardManager>().DisplayUpgrades();
    }

    private void OnDisable()
    {
        if (spawnRoutine != null)
            StopCoroutine(spawnRoutine);
    }


    public void DestroyEnemy()
    {
        enemyCount--;
        CheckWaveFinished();
    }

    public void CheckWaveFinished()
    {
        if (spawning || waitingEnemies > 0)
            return;

        if ((enemyCount <= chosenWave.contThreshold && waveIndex < chosenWave.allWaves.Length - 1)
            || enemyCount <= 0)
        {
            if(backupCheckRoutine != null)
            {
                StopCoroutine(backupCheckRoutine);
                backupCheckRoutine = null;
            }
            WaveFinished();
        }
    }

    public bool HasSpecialUpgrades()
    {
        if(chosenWave is null)
            return false;

        return chosenWave.specialRewards.Count > 0;
    }
    public List<UpgradeObject> GetSpecialUpgrades()
    {
        if (chosenWave is null)
            return null;

        return chosenWave.specialRewards;
    }

    private IEnumerator CheckCount()
    {
        while(!gaveUpgrade)
        {
            CheckWaveFinished();


            yield return new WaitForSeconds(1f);
        }
    }


    public void SpawnedEnemy()
    {
        waitingEnemies--;
        enemyCount++;
    }
    public void ReturnEnemy(GameObject enemy)
    {
        spawnQueue.Enqueue(enemy);
        waitingEnemies--;
    }

    private bool SpawnpointsEmpty()
    {
        foreach(SpawnPoint sp in spawnPoints)
        {
            if(sp.IsLoaded())
            {
                return false;
            }
        }
        return true;
    }
}
