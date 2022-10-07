using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDespawner : MonoBehaviour
{
    private float duration;
    private float worldTime;
    private float timer;
    ParticleSystem.MainModule main;

    void Start()
    {
        main = GetComponent<ParticleSystem>().main;
        duration = main.duration;
    }

    void Update()
    {
        worldTime = TimeManager.worldTime;

        main.simulationSpeed = 1 * worldTime;

        if (timer >= duration)
            Destroy(gameObject);
        else
            timer += Time.deltaTime * worldTime;
    }
}
