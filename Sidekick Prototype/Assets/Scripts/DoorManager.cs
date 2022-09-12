using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public List<FieldTrigger> doorways;
    public List<FieldTrigger> exits;
    public FieldTrigger entrance;


    public int enemies;
    private bool locked = false;

    private void Awake()
    {
        FieldTrigger[] temp = FindObjectsOfType<FieldTrigger>();
        enemies = FindObjectsOfType<TargetDummy>().Length;

        foreach(FieldTrigger f in temp)
        {
            if(f.type == FieldTrigger.FieldType.Entrance && entrance is null)
            {
                entrance = f;
            }
            else if(f.type == FieldTrigger.FieldType.Exit)
            {
                exits.Add(f);
            }
            else
            {
                doorways.Add(f);
            }
        }

        // If no set enterance found, get one from doorways
        if(entrance == null)
        {
            entrance = doorways[Random.Range(0, doorways.Count)];
            doorways.Remove(entrance);
        }
        entrance.SetEntrance();

        // If no exits, set doors to exits. Otherwise, disable remaining doors
        if(exits.Count <= 0)
        {
            // If no exits, choose doors from doorways
            foreach (FieldTrigger f in doorways)
            {
                f.SetExit();
                exits.Add(f);
            }
            doorways.Clear();
        }
        else
        {
            // If set exits, lock remaining doors
            foreach (FieldTrigger f in doorways)
            {
                f.LockDoor();
            }
        }


        locked = true;
        // If not an empty room, lock the remaining doors
        if (enemies > 0)
        {
            foreach (FieldTrigger f in exits)
            {
                f.LockDoor();
            }
        }
    }

    public void DestroyEnemy()
    {
        enemies--;
    }

    private void FixedUpdate()
    {
        if(enemies <= 0 && locked)
        {
            locked = false;
            foreach (FieldTrigger f in exits)
            {
                f.UnlockDoor();
            }
        }
    }
}
