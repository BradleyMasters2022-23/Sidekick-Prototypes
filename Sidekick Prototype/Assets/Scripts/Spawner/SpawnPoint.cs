using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private bool usable = true;

    public bool Usable()
    {
        // If frozen in time, dont spawn
        if (TimeManager.worldTime == 0f)
            return false;


        return usable;
    }

    public Transform Pos()
    {
        return gameObject.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        usable = false;
    }
    private void OnTriggerExit(Collider other)
    {
        usable = true;
    }
}
