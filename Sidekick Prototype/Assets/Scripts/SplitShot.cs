using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitShot : PProjectile
{

    protected override void Start()
    {
        base.Start();
        transform.DetachChildren();
    }

}
