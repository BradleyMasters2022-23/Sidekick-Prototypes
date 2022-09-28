using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleFireSlowUpgrade : IUpgrade
{
    private bool loaded = false;
    public float firerateIncrease;
    private PlayerGun[] gun;
    private float originalFirerate;
    private bool addedFirerate = false;

    public override void LoadUpgrade(PlayerControllerRB player)
    {
        gun = FindObjectsOfType<PlayerGun>(true);
        originalFirerate = gun[0].fireDelay;
        loaded = true;
    }

    private void Update()
    {
        if(loaded)
        {
            if(TimeManager.worldTime <= 0.5f && !addedFirerate)
            {
                addedFirerate = true;

                foreach (PlayerGun t in gun)
                {
                    t.fireDelay = (originalFirerate / firerateIncrease);
                }
            }
            else if (TimeManager.worldTime > 0.5f && addedFirerate)
            {
                addedFirerate = false;

                foreach (PlayerGun t in gun)
                {
                    t.fireDelay = originalFirerate;
                }
            }
        }
    }
}
