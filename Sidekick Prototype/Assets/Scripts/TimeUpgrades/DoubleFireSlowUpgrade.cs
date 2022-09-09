using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleFireSlowUpgrade : IUpgrade
{
    private bool loaded = false;
    public float firerateIncrease;
    private PlayerGun gun;
    private float originalFirerate;
    private bool addedFirerate = false;

    public override void LoadUpgrade(PlayerControllerRB player)
    {
        gun = FindObjectOfType<PlayerGun>(); ;
        originalFirerate = gun.fireDelay;
        loaded = true;
    }

    private void Update()
    {
        if(loaded)
        {
            if(TimeManager.worldTime == 0 && !addedFirerate)
            {
                addedFirerate = true;
                gun.fireDelay = (originalFirerate / firerateIncrease);
            }
            else if (TimeManager.worldTime != 0 && addedFirerate)
            {
                addedFirerate = false;
                gun.fireDelay = originalFirerate;
            }
        }
    }
}
