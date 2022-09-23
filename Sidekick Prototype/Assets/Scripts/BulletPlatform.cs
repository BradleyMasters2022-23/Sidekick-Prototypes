using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPlatform : MonoBehaviour
{

    private Collider c;
    private float currTime;
    private bool bulletMode = true;

    private int defLayer;
    private string platformLayer = "Ground";

    private void Awake()
    {
        c = GetComponent<Collider>();
        defLayer = this.gameObject.layer;

        if (currTime <= 0)
            PlatformMode();
    }

    // Update is called once per frame
    void Update()
    {
        currTime = TimeManager.worldTime;

        if(currTime <= 0 && bulletMode)
        {
            PlatformMode();
        }
        else if (currTime > 0 && !bulletMode)
        {
            BulletMode();
        }
    }

    private void PlatformMode()
    {
        gameObject.layer = LayerMask.NameToLayer(platformLayer);


        bulletMode = false;
        c.isTrigger = false;
    }

    private void BulletMode()
    {
        gameObject.layer = defLayer;
        bulletMode = true;
        c.isTrigger = true;
    }
}
