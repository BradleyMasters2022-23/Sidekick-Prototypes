using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDummy : IDamagable
{
    protected override void Awake()
    {
        GetComponent<Renderer>().material.color = Color.red;
        base.Awake();

        // If dummy cannot be killed, make sure to remove from pool
        if(invulnerable)
            FindObjectOfType<DoorManager>().DestroyEnemy();
    }


    public override void Die()
    {
        base.Die();
        FindObjectOfType<DoorManager>().DestroyEnemy();
    }
}
