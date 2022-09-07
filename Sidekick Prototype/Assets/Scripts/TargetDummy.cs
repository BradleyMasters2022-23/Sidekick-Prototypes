using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDummy : IDamagable
{
    protected override void Awake()
    {
        GetComponent<Renderer>().material.color = Color.red;
        base.Awake();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
